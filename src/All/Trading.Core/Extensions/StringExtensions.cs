using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsUsdt(this string str) => str.ToUpperInvariant() == "USDT";

        public static bool IsAssetEqual(this string str, string asset) => str.ToUpperInvariant() == asset.ToUpperInvariant();
    }
}
