using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;
using Trading.Bot.Domain.Contracts.Enums;

namespace Trading.Bot.Core.Settings
{
    public class StrategySettings
    {
        public string Contract { get; set; } = string.Empty;
        public decimal Capital { get; set; }
        public decimal Slippage { get; set; }
        public OrderType OrderType { get; set; }
        public Dictionary<TimeSpan, TimeframeSettings> TimeframeSettings { get; set; }
            = new Dictionary<TimeSpan, TimeframeSettings>();
    }

    public class TimeframeSettings
    {
        public TimeSpan Timeframe { get; set; }
        public decimal PartOfCapital { get; set; }
        public bool IsReinvesiting { get; set; }
        public int? PeriodEma { get; set; }
        public TypeLimit TypeLimit { get; set; }
        public decimal? TakeProfitLong { get; set; }
        public decimal? StopLossLong { get; set; }
        public decimal? TakeProfitShort { get; set; }
        public decimal? StopLossShort { get; set; }
    }

    public enum TypeLimit
    {
        PP,
        Percent
    }
}
