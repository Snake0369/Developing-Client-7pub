using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts.Enums;

namespace Trading.Bot.Domain.Contracts
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public Guid OrderItemId { get; set; }
        public Guid ContractId { get; set; }
        public DateTime Created { get; set; }
        public Side Side { get; set; }
        public OrderType OrderType { get; set; }
        public decimal? Price { get; set; }
        public decimal Amount { get; set; } // Для спот рынков лот
        public decimal Balance { get; set; }
        public OrderState State { get; set; }
        public DateTime? Updated { get; set; }
        public virtual Contract? Contract { get; set; }
        public virtual OrderItem? OrderItem { get; set; }

        public OrderState GetOrderState() => State;
    }

    public class OrderComparer : IEqualityComparer<Order>
    {
        public bool Equals(Order? x, Order? y)
        {
            return x == null || y == null ? false : x.OrderId.Equals(y.OrderId);
        }

        public int GetHashCode([DisallowNull] Order obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}
