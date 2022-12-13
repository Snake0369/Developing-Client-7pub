using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class BarHistory
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Timeframe { get; set; }
        public Guid ContractId { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public virtual Contract? Contract { get; set; }

        public DateTime GetDate() => Timestamp.Date;
    }

    public class BarHistoryComparer : IEqualityComparer<BarHistory>
    {
        public bool Equals(BarHistory? x, BarHistory? y)
        {
            return x == null || y == null ? false : x.Timestamp.Equals(y.Timestamp);
        }

        public int GetHashCode([DisallowNull] BarHistory obj)
        {
            return obj == null ? 0 : obj.GetHashCode();
        }
    }
}
