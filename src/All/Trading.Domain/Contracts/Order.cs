using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Contracts.Enums;

namespace Trading.Domain.Contracts
{
    public class Order
    {
        public Guid Id { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public Enums.Exchange? Exchange { get; set; } = null;
        public DateTime? Timestamp { get; set; }
        public Guid SubpositionId { get; set; }
        public string BaseAsset { get; set; } = string.Empty;
        public string QuoteAsset { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public Side Side { get; set; }
        public string? ClientOrderId { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountRemaining { get; set; }
        public decimal? AmountFilled { get; set; }
        public OrderStatus? Status { get; set; } = null;
        public string Type { get; set; } = string.Empty;
        public bool IsMargin { get; set; }
        public virtual Subposition? Subposition { get; set; }
    }
}
