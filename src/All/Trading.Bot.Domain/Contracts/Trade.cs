using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Trading.Bot.Domain.Contracts.Enums;

namespace Trading.Bot.Domain.Contracts
{
    public class Trade
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid OrderItemId { get; set; }
        public string TradeNum { get; set; } = string.Empty;
        public Side Side { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Commission { get; set; }
        public DateTime? Updated { get; set; }
        public virtual OrderItem? OrderItem { get; set; }
    }

    public class TradeComparer : IEqualityComparer<Trade>
    {
        public bool Equals(Trade? x, Trade? y)
        {
            return x == null || y == null ? false : x.Id.Equals(y.Id);
        }

        public int GetHashCode([DisallowNull] Trade obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}