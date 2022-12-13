using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts.Enums;

namespace Trading.Bot.Domain.Contracts
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid OrderChannelId { get; set; }
        public Guid SubpositionId { get; set; }
        public OrderItemState State { get; set; }
        public virtual List<Order> Orders { get; set; } = new List<Order>();
        public virtual List<Trade> Trades { get; set; } = new List<Trade>();
        public virtual OrderChannel? OrderChannel { get; set; }
        public virtual Subposition? Subposition { get; set; }

        public async Task AddOrder(Order order, Func<Order, Task> saveOrder)
        {
            Orders.Add(order);
            await saveOrder.Invoke(order);
        }

        public void AddTrade(Trade trade)
        {
            Trades.Add(trade);
        }

        public async Task AddTrade(DateTime timestamp, string tradeNum, Side side, decimal price, decimal amount, decimal commission, Func<Trade, Task> saveTrade)
        {
            var trade = new Trade
            {
                Id = Guid.NewGuid(),
                Created = timestamp,
                TradeNum = tradeNum,
                Side = side,
                Price = price,
                Amount = amount,
                Commission = commission,
                OrderItem = this,
                OrderItemId = Id,
                Updated = DateTime.Now
            };
            AddTrade(trade);
            await saveTrade.Invoke(trade);
        }
    }
}
