using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Contracts.Enums;

namespace Trading.Domain.Contracts
{
    public class FuturePosition
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid ExchangeId { get; set; }
        public virtual Exchange? Exchange { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string? PositionId { get; set; }
        public Side Side { get; set; }
        public decimal? AvailableBalance { get; set; }
        public decimal? EntryPrice { get; set; }
        public decimal? MarkPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal? LiquidationPrice { get; set; }
        public decimal? BankruptcyPrice { get; set; }
        public decimal? Leverage { get; set; }
        public decimal? RealizedPnL { get; set; }
        public decimal? UnrealizedPnL { get; set; }
        public decimal? TotalRealizedPnL { get; set; }
        public decimal? TakeProfitPrice { get; set; }
        public decimal? WalletBalance { get; set; }
        public PositionStatus? PositionStatus { get; set; }
        public MarginType? MarginType { get; set; }

    }
}
