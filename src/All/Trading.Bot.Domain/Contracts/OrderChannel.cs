using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class OrderChannel
    {
        public Guid Id { get; set; }
        public virtual List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public async Task AddOrderItem(OrderItem order, Func<OrderItem, Task> saveOrderItem)
        {
            Items.Add(order);
            await saveOrderItem.Invoke(order);
        }
    }
}
