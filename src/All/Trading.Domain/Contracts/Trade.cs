using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Contracts.Enums;

namespace Trading.Domain.Contracts
{
    public class Trade
    {
        public Guid Id { get; set; }
        public string TradeId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public Enums.Exchange Exchange { get; set; }
        public Guid SubpositionId { get; set; }
        public string BaseAsset { get; set; } = string.Empty;
        public string QuoteAsset { get; set; } = string.Empty;
        public string? Symbol { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string? ClientOrderId { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Fee { get; set; }
        public string? FeeAsset { get; set; } = null;
        public virtual Subposition? Subposition { get; set; }

        public async Task<decimal> GetFee(Func<string?, decimal, Task<decimal>> returnFee)
        {
            if (Subposition != null)
            {
                return await returnFee(FeeAsset, Fee);
            }
            return 0;
        }
    }
}
