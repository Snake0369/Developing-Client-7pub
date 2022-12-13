using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Core.Context
{
    public class DbContextService<TContext>
        where TContext : DbContext
    {
        private readonly DbContextOptions<TContext> _options;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="DbContextService{T}"/>
        /// </summary>
        /// <param name="options">
        /// Настройки, используемые для инициализации контекста.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> равен <see langword="null"/>.
        /// </exception>
        // TODO: This is a wrong way to pass DbContext options, pass IServiceScopeFactory instead
        public DbContextService(DbContextOptions<TContext> options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        /// <summary>
        /// Создает новый экземпляр контекста.
        /// </summary>
        /// <param name="options">
        /// Настройки, используемые для инициализации контекста.
        /// </param>
        /// <returns>
        /// Экземпляр контекста.
        /// </returns>
        public virtual TContext CreateContext(DbContextOptions<TContext> options)
        {
            var type = typeof(TContext);
            ConstructorInfo? ctor = type.GetConstructor(new[] { typeof(DbContextOptions<TContext>) });

            return (TContext)ctor?.Invoke(new object[] { options });
        }

        public TContext CreateContext()
        {
            var type = typeof(TContext);
            ConstructorInfo? ctor = type.GetConstructor(new[] { typeof(DbContextOptions<TContext>) });

            return (TContext)ctor?.Invoke(new object[] { _options });
        }

        /// <summary>
        /// Выполняет асинхронно функцию в области действия контекста.
        /// </summary>
        /// <param name="func">
        /// Выполняемый асинхронный блок кода.
        /// </param>
        public async Task UseContextAsync(Func<TContext, Task> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            await using (var context = CreateContext(_options))
            {
                await func(context).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Выполняет асинхронно функцию в области действия контекста с возвращением результата.
        /// </summary>
        /// <param name="func">
        /// Выполняемый асинхронный блок кода.
        /// </param>
        /// <returns>
        /// Результат выполнения функции
        /// </returns>
        /// <typeparam name="T">
        /// Тип возвращаемого результата.
        /// </typeparam>
        public async Task<T> UseContextAsync<T>(Func<TContext, Task<T>> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            try
            {
                using (var context = CreateContext(_options))
                {
                    return await func(context).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                if (e.Message != "08P01: server conn crashed?")
                {
                    Console.WriteLine(e);
                    throw;
                }
                await Task.Delay(500);
#if DEBUG
                Console.WriteLine("Подождали пол секунды, делаем повторный запрос к PG");
#endif
                using (var context = CreateContext(_options))
                {
                    return await func(context).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Выполняет асинхронно функцию в области действия контекста, в завершении вызывая сохранение изменений.
        /// </summary>
        /// <param name="func">
        /// Выполняемый асинхронный блок кода.
        /// </param>
        public async Task UseContextSavingChangesAsync(Func<TContext, Task> func, CancellationToken token = default) =>
            await UseContextAsync(
                async context =>
                {
                    await func(context).ConfigureAwait(false);
                    await context.SaveChangesAsync(token).ConfigureAwait(false);
                })
                .ConfigureAwait(false);

        public async Task UseContextSavingChangesAsync(Action<TContext?> func, CancellationToken token = default) =>
            await UseContextAsync(
                async context =>
                {
                    func(context);
                    if (context != null)
                    {
                        await context.SaveChangesAsync(token).ConfigureAwait(false);
                    }
                })
                .ConfigureAwait(false);



        public async Task Insert<TEntity>(TEntity entity, CancellationToken token = default) where TEntity : class
        {
            await UseContextSavingChangesAsync(db =>
            {
                if (db != null)
                {
                    db.Entry(entity).State = EntityState.Added;
                }
            }, token);
        }

        public async Task Insert<TEntity>(IEnumerable<TEntity> entities, CancellationToken token = default) where TEntity : class
        {
            await UseContextSavingChangesAsync(db =>
            {
                if (db != null)
                {
                    foreach (TEntity entity in entities)
                    {
                        db.Entry(entity).State = EntityState.Added;
                    }
                }
            }, token);
        }

        public async Task Update<TEntity>(TEntity entity, CancellationToken token = default) where TEntity : class
        {
            await UseContextSavingChangesAsync(db =>
            {
                if (db != null)
                {
                    db.Entry(entity).State = EntityState.Modified;
                }
            }, token);
        }

        public async Task Delete<TEntity>(TEntity entity, CancellationToken token = default) where TEntity : class
        {
            await UseContextSavingChangesAsync(db =>
            {
                if (db != null)
                {
                    db.Entry(entity).State = EntityState.Deleted;
                }
            }, token);
        }

        public async Task Delete<TEntity>(IEnumerable<TEntity> entities, CancellationToken token = default) where TEntity : class
        {
            await UseContextSavingChangesAsync(db =>
            {
                if (db != null)
                {
                    foreach (var entity in entities)
                    {
                        db.Entry(entity).State = EntityState.Deleted;
                    }
                }
            }, token);
        }
    }
}
