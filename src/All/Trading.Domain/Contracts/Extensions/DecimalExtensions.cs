using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts.Extensions
{
    public static class DecimalExtensions
    {
        private static readonly decimal Fee = 1M;
        public static decimal AppropriatedToFee(this decimal source)
        {
            return source * (1 + Fee / 100);
        }

        public static decimal FloorAmountToMinLot(this decimal amount, decimal minLot)
        {
            return Math.Floor(amount / minLot) * minLot;
        }

        public static decimal CellingAmountToMinLot(this decimal amount, decimal minLot)
        {
            return Math.Ceiling(amount / minLot) * minLot;
        }

        public static decimal RoundAmountToMinLot(this decimal amount, int decimals)
        {
            return Math.Round(amount, decimals);
        }
    }
}
