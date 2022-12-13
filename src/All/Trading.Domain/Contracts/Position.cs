using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Contracts.Enums;
using Trading.Domain.Contracts.Extensions;

namespace Trading.Domain.Contracts
{
    public class Position
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid StrategyId { get; set; }
        public string BaseAsset { get; set; } = default!;
        public string QuoteAsset { get; set; } = default!;
        public string Symbol { get; set; } = default!;
        public Side Side { get; set; }
        public decimal Amount { get; set; }
        public State State { get; set; }
        public DateTime? TimestampTakeOn { get; set; }
        public bool IsTakeOn { get; set; } = false;
        public DateTime? TimestampStop { get; set; }
        public decimal? Stop { get; set; }
        public virtual List<Subposition> Subpositions { get; set; } = new List<Subposition>();
        public virtual Strategy? Strategy { get; set; }

        public static Position Create(Strategy strategy, string baseAsset, string quoteAsset, string symbol, Side side, decimal amountUsdt, DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.UtcNow;
            }
            return new Position
            {
                Id = Guid.NewGuid(),
                StrategyId = strategy.Id,
                Timestamp = date.Value,
                BaseAsset = baseAsset,
                QuoteAsset = quoteAsset,
                Symbol = symbol,
                Side = side,
                Amount = amountUsdt,
                State = State.Open,
                Strategy = strategy
            };
        }

        public Subposition Add(Side side, decimal amount, Func<Subposition, Task>? function = null, DateTime? date = null, Subposition? link = null)
        {
            var subposition = Subposition.Add(this, SubpositionType.Trade, side, BaseAsset, QuoteAsset, Symbol, amount, function, date, link);
            AddSubposition(subposition);
            return subposition;
        }

        public Subposition Add(Side side, decimal amount, Func<Subposition, Task<string?>>? function = null, DateTime? date = null, Subposition? link = null)
        {
            var subposition = Subposition.Add(this, SubpositionType.Trade, side, BaseAsset, QuoteAsset, Symbol, amount, function, date, link);
            AddSubposition(subposition);
            return subposition;
        }

        public async Task<Subposition?> Close(Func<Position, Task>? function1 = null, Func<Subposition, Task>? function2 = null)
        {
            if (Subpositions.Any())
            {
                var amount = Subpositions.Sum(x => x.GetQuantity());
                if (amount != null)
                {
                    var subposition = Subposition.Add(this, SubpositionType.Trade, Side.Invert(), BaseAsset, QuoteAsset, Symbol, amount.Value, function2);
                    Subpositions.Add(subposition);
                    if (function1 != null)
                    {
                        await function1.Invoke(this);
                    }
                    return subposition;
                }
            }
            return null;
        }

        public async Task<Subposition?> Close(Func<Position, Task>? function1 = null, Func<Subposition, Task<string?>>? function2 = null)
        {
            if (Subpositions.Any())
            {
                var amount = Subpositions.Sum(x => x.GetQuantity());
                if (amount != null)
                {
                    var subposition = Subposition.Add(this, SubpositionType.Trade, Side.Invert(), BaseAsset, QuoteAsset, Symbol, amount.Value, function2);
                    Subpositions.Add(subposition);
                    if (function1 != null)
                    {
                        await function1.Invoke(this);
                    }
                    return subposition;
                }
            }
            return null;
        }

        public List<Subposition> GetSubpositions() => Subpositions.ToList();

        public decimal? GetProfit(decimal price)
        {
            if (GetSubpositions().All(x => x.State == SubpositionState.Implemented))
            {
                var cost = GetSubpositions().Sum(x => x.GetCost());
                var quantity = GetSubpositions().Sum(x => x.GetQuantity());
                var fee = GetSubpositions().Sum(x => x.GetFee("USDT"));
                if (cost != null && quantity != null)
                {
                    return cost.Value + quantity.Value * price - fee;
                }
            }
            return null;
        }

        public async Task<decimal> GetFees(Func<string?, decimal, Task<decimal>> returnFee)
        {
            return await Subpositions.ToAsyncEnumerable().SumAwaitAsync(async x => await x.GetFee(returnFee));
        }

        private void AddSubposition(Subposition subposition)
        {
            Subpositions.Add(subposition);
        }
    }
}
