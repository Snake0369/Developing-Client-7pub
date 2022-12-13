using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts.Enums
{
    public enum Exchange
    {
        Undefined,
        Kraken,
        Huobi,
        Binance,
        BinanceUsdFutures,
        Bybit,
        BybitUsdPerpetual,
        Bittrex,
        Coinbase,
        BinanceCoinFutures,
        BybitInversePerpetual
    }
}
