using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Core.Static
{
    public static class BotSettings
    {
        public static string BotId { get; set; } = default!;
        public static string BotIdMM2 { get; set; } = default!;
        public static string BotName { get; set; } = default!;
        public static string BinanceApiKey { get; set; } = default!;
        public static string BinanceApiSecret { get; set; } = default!;
        public static string BaseAsset { get; set; } = default!;
        public static string QuoteAsset { get; set; } = default!;
        public static string Symbol { get; set; } = default!;
        public static decimal StartCapital { get; set; } = default!;
        public static decimal SLXForward { get; set; } = default!;
        public static decimal SLXBack { get; set; } = default!;
        public static decimal LimitOfNoOpenPositionReturn { get; set; } = default!;
        public static decimal LimitOfOpenPositionReturn { get; set; } = default!;
    }
}


