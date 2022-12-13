using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Trading.Bot.Domain.Contracts;

namespace Trading.Core.Extensions
{
    public static class LinqExtensions
    {
        public static decimal? Beta(this List<decimal> source,
            List<decimal> second)  
        {
            if (second == null) throw new ArgumentNullException("source");
            if (source.Count != 0 && source.Count == second.Count)
            {
                var mOFirst = (double)source.Sum() / source.Count;
                var mOFSecond = (double)second.Sum() / second.Count;
                var sum = 0M;
                for (var i = 0; i < source.Count; i++)
                {
                    sum += source[i] * second[i];
                }
                var cov = (double)sum / source.Count - mOFirst * mOFSecond;
                var disp = 0D;
                for (var i = 0; i < second.Count; i++)
                {
                    disp += Math.Pow((double)second[i] - mOFSecond, 2);
                }
                disp /= second.Count;

                return new decimal(cov / disp);
            }
            return null;
        }

        public static List<decimal> ToProfitability(this List<decimal> source)
        {
            var result = new List<decimal>();
            if (source.Any() && source.Count > 1)
            {
                for (var i = 1; i < source.Count; i++)
                {
                    result.Add(new decimal(Math.Log((double)source[i] / (double)source[i-1])));
                }
            }
            return result;
        }

        public static decimal? Ema(this List<decimal> source)
        {
            if (source.Any() && source.Count > 1)
            {
                var k = 2M / (source.Count + 1);
                var ema = source.First();
                for (var i = 1; i < source.Count; i++)
                {
                    ema = k * source[i] + (1 - k) * ema;
                }
                return ema;
            }
            return default(decimal?);
        }

        public static decimal? Sma(this List<decimal> source)
        {
            if (source.Any())
            {
                return source.Average();
            }
            return default(decimal?);
        }

        public static (decimal? K, decimal? D) Stoch(this List<Bar> source, int kPeriod, int dPeriod)
        {
            if (source.Any() && source.Count >= (kPeriod + dPeriod))
            {
                var k = new List<decimal>();
                for (int i = 0; i < source.Count - kPeriod; i++)
                {
                    var arr = source.Skip(i + 1).Take(kPeriod);
                    var close = arr.Last().Close;
                    var min = arr.Min(b => b.Low);
                    var max = arr.Max(b => b.High);
                    k.Add(100M * ((close - min) / (max - min)));
                }
                if (k.Count >= dPeriod)
                {
                    var d = k.TakeLast(dPeriod).Average();
                    return (k.Last(), d);
                }
            }
            return (null, null);
        }

        public static bool IsOnlySameContract(this List<BarHistory> bars)
        {
            var contractId = bars.Select(x => x.ContractId).FirstOrDefault();
            return bars.All(x => x.ContractId == contractId);
        }

        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(
            this IEnumerable<TSource> source, int size)
        {
            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                {
                    bucket = new TSource[size];
                }

                bucket[count++] = item;
                if (count != size)
                {
                    continue;
                }

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
            {
                yield return bucket.Take(count);
            }
        }

        /// <summary>
        /// Функция для асинхронной обработки последовательности в несколько потоков с ограничением количества элементов в одном цикле обработки
        /// </summary>
        public static async Task<IReadOnlyCollection<TResult>> ParallelAsyncBatchProcessing<TIn, TResult>(
            this IEnumerable<TIn> items,
            int batchSize,
            int parrallelCount,
            Func<IEnumerable<TIn>, Task<IReadOnlyCollection<TResult>>> func)
        {
            var batchedItems = items.Batch(batchSize).ToList();
            var result = await ParallelAsyncProcessingMany(batchedItems, parrallelCount, func);

            return result;
        }

        public static async Task ParallelAsyncBatchProcessing<TIn>(
            this IEnumerable<TIn> items,
            int batchSize,
            int parrallelCount,
            Func<IEnumerable<TIn>, Task> func)
        {
            var batchedItems = items.Batch(batchSize).ToList();
            await ParallelAsyncProcessing(batchedItems, parrallelCount, func);
        }
        /// <summary>
        /// Параллельная асинхронная обработка последовательности.
        /// </summary>
        /// <typeparam name="TIn">Тип элементов исходной последовательности</typeparam>
        /// <typeparam name="TResult">Тип элементов результирующей последовательности</typeparam>
        /// <param name="items">Элементы исходной последовательности</param>
        /// <param name="parrallelCount">Количество одновременных потоков</param>
        /// <param name="func">Функция, которая для одного элемента последовательности выдаёт массив результатов</param>
        /// <returns></returns>
        public static async Task<IReadOnlyCollection<TResult>> ParallelAsyncProcessingMany<TIn, TResult>(
            this IEnumerable<TIn> items,
            int parrallelCount,
            Func<TIn, Task<IReadOnlyCollection<TResult>>> func)
        {
            var concurrentResult = new ConcurrentQueue<TResult>();
            var concurrentSource = new ConcurrentQueue<TIn>(items);

            var tasks = Enumerable.Range(0, parrallelCount).Select(async x =>
            {
                var maxRepeat = concurrentSource.Count;
                for (int i = 0; i < maxRepeat; ++i)
                {
                    if (!concurrentSource.TryDequeue(out var item))
                    {
                        return;
                    }

                    var subResults = await func(item);

                    foreach (var subResult in subResults)
                    {
                        concurrentResult.Enqueue(subResult);
                    }
                }
            });

            await Task.WhenAll(tasks);

            return concurrentResult;
        }

        /// <summary>
        /// Параллельная обработка элементов с помощью функции и складывание их в единый массив
        /// </summary>
        public static async Task<IReadOnlyCollection<TResult>> ParallelAsyncProcessing<TIn, TResult>(
            this IEnumerable<TIn> items,
            int parrallelCount,
            Func<TIn, Task<TResult>> func)
        {
            var concurrentResult = new ConcurrentQueue<TResult>();
            var concurrentSource = new ConcurrentQueue<TIn>(items);

            var tasks = Enumerable.Range(0, parrallelCount).Select(async x =>
            {
                var maxRepeat = concurrentSource.Count;
                for (int i = 0; i < maxRepeat; ++i)
                {
                    if (!concurrentSource.TryDequeue(out var item))
                    {
                        return;
                    }

                    var subResult = await func(item);
                    concurrentResult.Enqueue(subResult);
                }
            });

            await Task.WhenAll(tasks);

            return concurrentResult;
        }

        public static async Task ParallelAsyncProcessing<TIn>(
            this IEnumerable<TIn> items,
            int parrallelCount,
            Func<TIn, Task> func)
        {
            var concurrentSource = new ConcurrentQueue<TIn>(items);

            var tasks = Enumerable.Range(0, parrallelCount).Select(async x =>
            {
                while (concurrentSource.TryDequeue(out var item))
                {
                    await func(item);
                }
            });

            await Task.WhenAll(tasks);
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static bool In<T>(this T x, params T[] set)
        {
            return set.Contains(x);
        }

        public static bool ContainsOneOf<T>([NotNull] this IReadOnlyCollection<T> sourceItems, IEnumerable<T> targetItems)
        {
            if (sourceItems == null)
            {
                throw new ArgumentNullException(nameof(sourceItems));
            }

            foreach (var k in targetItems)
            {
                if (sourceItems.Contains(k))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ContainsOneOf<T>([NotNull] this IReadOnlyCollection<T> sourceItems, params T[] targetItems)
        {
            return sourceItems.ContainsOneOf((IEnumerable<T>)targetItems);
        }

    }
}