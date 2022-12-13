using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts.Enums;

namespace Trading.Bot.Domain.Contracts
{
    public class Subposition
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid PositionId { get; set; }
        public Guid OrderChannelId { get; set; }
        public Side Side { get; set; }
        public OrderType OrderType { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Quantity { get; set; }
        public DateTime LastUpdated { get; set; }
        public SubpositionState State { get; set; }
        public virtual OrderChannel? Channel { get; set; } 
        public virtual Position? Position { get; set; }

        public static Subposition Add(Position position, OrderChannel channel, Side side, decimal price, decimal amount, decimal quantity, OrderType orderType, DateTime? date = null)
        {
            return new Subposition
            {
                Id = Guid.NewGuid(),
                Created = date == null ? DateTime.Now : date.Value,
                PositionId = position.Id,
                Side = side,
                Price = price,
                Amount = amount,
                Quantity = quantity,
                OrderType = orderType,
                LastUpdated = date == null ? DateTime.Now : date.Value,
                Position = position,
                Channel = channel,
                State = SubpositionState.NotImplemented
            };
        }

        public async Task SetState(SubpositionState state, Func<Subposition, Task>? func = null)
        {
            State = state;
            if (func != null)
            {
                await func.Invoke(this);
            }
        }

        public decimal GetCost()
        {
            var cost = Channel?.Items
                .Where(x => x.SubpositionId == Id)
                .SelectMany(x => x.Trades)
                .Sum(x => x.Amount * x.Price);
            if (cost == null)
            {
                return 0;
            }
            return Side == Side.Buy ? -cost.Value : cost.Value;
        }

        public decimal GetQuantity()
        {
            var amount = Channel?.Items
                .Where(x => x.SubpositionId == Id)
                .SelectMany(x => x.Trades)
                .Sum(x => x.Amount);
            if (amount == null)
            {
                return 0;
            }
            return Side == Side.Sell ? -amount.Value : amount.Value;
        }

        public decimal GetAmount() => Side == Side.Buy ? Amount : -Amount;
    }
}
