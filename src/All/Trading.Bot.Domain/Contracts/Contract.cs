using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class Contract
    {
        public Guid Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public string BaseTicker { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Expired { get; set; }
        public DateTime LastTradingDate { get; set; }
        public virtual List<BarHistory> BarHistories { get; set; } = new List<BarHistory>();
        public virtual List<TradeHistory> TradeHistories { get; set; } = new List<TradeHistory>();
        public virtual List<Renko> Renkos { get; set; } = new List<Renko>();
    }
}