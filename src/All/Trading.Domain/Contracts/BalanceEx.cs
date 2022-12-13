using CryptoExchange.Net.CommonObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts
{
    public class BalanceEx
    {
        public Guid Id { get; set; }
        public DateTime? Updated { get; set; }
        public string? Asset { get; set; } = null;
        public decimal Available { get; set; }
        public decimal? Total { get; set; }
        public bool IsMargin { get; set; }
        public decimal WalletBalance { get; set; }
        public decimal RealizedPnL { get; set; }
        public decimal UnrealizedPnL { get; set; }
        public decimal TotalRealizedPnL { get; set; }
    }
}
