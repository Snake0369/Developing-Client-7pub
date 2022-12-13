using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts.Enums;
using Trading.Bot.Domain.Extensions;

namespace Trading.Bot.Domain.Contracts
{
    public class Position
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid StrategyId { get; set; }
        public Guid ContractId { get; set; }
        public int Timeframe { get; set; }
        public decimal Amount { get; set; }
        public Side Side { get; set; }
        public State State { get; set; }
        public DateTime? LastUpdated { get; set; }
        public virtual List<Subposition> Subpositions { get; set; } = new List<Subposition>();
        public virtual Strategy? Strategy { get; set; }
        public virtual Contract? Contract { get; set; }


        private readonly decimal _commission = 1;

        public static Position Create(Strategy strategy, Contract contract, TimeSpan timeframe, Side side, decimal amount, DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            return new Position
            {
                Id = Guid.NewGuid(),
                StrategyId = strategy.Id,
                Created = date.Value,
                ContractId = contract.Id,
                Timeframe = (int)timeframe.TotalMinutes,
                Side = side,
                Amount = amount,
                Contract = contract
            };
        }

        public async Task Open(OrderChannel channel, decimal amount, decimal price, OrderType orderType, DateTime? date = null, Func<Position, Task>? func1 = null, Func<Subposition, Task>? func2 = null, Func<Subposition, Task>? func3 = null)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            LastUpdated = date.Value;
            State = State.Open;
            var quantity = (int)(Amount * amount / price);
            var subposition = Subposition.Add(this, channel, Side, price, amount, quantity, orderType, date);
            AddSubposition(subposition);
            await subposition.SetState(SubpositionState.NotImplemented, func3);
            if (func1 != null && func2 != null)
            {
                await func1.Invoke(this);
                await func2.Invoke(subposition);
            }
        }

        public async Task Update(Side side, decimal amount, decimal quantity, decimal price, DateTime? date = null, Func<Position, Task>? func1 = null, Func<Subposition, Task>? func2 = null, Func<Subposition, Task>? func3 = null)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            LastUpdated = date.Value;
            if (Subpositions.Any())
            {
                var subposition = Subposition.Add(this, Subpositions.Last().Channel, side, price, amount, quantity, Subpositions.Last().OrderType, date);
                AddSubposition(subposition);
                await subposition.SetState(SubpositionState.NotImplemented, func3);
                if (func2 != null)
                {
                    await func2.Invoke(subposition);
                }
            }
            if (func1 != null)
            {
                await func1.Invoke(this);
            }
        }

        public async Task Close(decimal price, DateTime? date = null, Func<Position, Task>? func1 = null, Func<Subposition, Task>? func2 = null, Func<Subposition, Task>? func3 = null)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            LastUpdated = date.Value;
            State = State.Closed;
            if (Subpositions.Any())
            {
                if (GetQuantity() != 0)
                {
                    var quantity = Math.Abs(GetQuantity());
                    var subposition = Subposition.Add(this, Subpositions.Last().Channel, Side.Invert(), price, Math.Abs(GetAmount()), quantity, Subpositions.Last().OrderType, date);
                    AddSubposition(subposition);
                    await subposition.SetState(SubpositionState.NotImplemented, func3);
                    if (func2 != null)
                    {
                        await func2.Invoke(subposition);
                    }
                }
            }
            if (func1 != null)
            {
                await func1.Invoke(this);
            }
        }

        public decimal GetProfit(decimal? price = null)
        {
            if (State == State.Open && price == null)
            {
                throw new InvalidOperationException("Для открытой позиции текущая цена контракта должна быть задана");
            }
            var cost = Subpositions.Sum(s => s.GetCost());
            var amount = Subpositions.Sum(s => s.GetQuantity());
            return price == null ? cost : cost + amount * price.Value - Math.Abs(amount * _commission);
        }

        public decimal GetQuantity() => Subpositions
            .ToList()
            .Sum(s => s.GetQuantity());

        public decimal GetAmount() => Subpositions.ToList().Sum(s => s.GetAmount());

        public decimal? GetEntryPrice()
        {
            var subpositionSameDirection = Subpositions.Where(s => s.Side == Side);
            if (subpositionSameDirection.All(s => s.State == SubpositionState.Implemented))
            {
                var cost = Math.Abs(subpositionSameDirection.Sum(s => s.GetCost()));
                var quantity = Math.Abs(subpositionSameDirection.Sum(s => s.GetQuantity()));
                if (quantity > 0)
                {
                    return cost / quantity;
                }
            }
            return null;
        }

        private void AddSubposition(Subposition subposition)
        {
            Subpositions.Add(subposition);
        }
    }
}
