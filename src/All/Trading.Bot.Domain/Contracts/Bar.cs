using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class Bar
    {
        public DateTime Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }

        public DateTime GetDate() => Timestamp.Date;

        public override string ToString()
        {
            return string.Concat($"[Bar {Timestamp:yy/MM/dd HH:mm}",
                "O:", Open,
                "H:", High,
                "L:", Low,
                "C:", Close,
                "V:", Volume, "]");
        }
    }
}
