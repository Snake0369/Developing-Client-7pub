using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class TradeAllDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string TradeNum { get; set; } = string.Empty;
        public uint Flags { get; set; }
        public string SecCode { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Value { get; set; }
        public DateTime? Updated { get; set; }

        public override string ToString()
        {
            return string.Concat("Time:", Timestamp.ToString("yy/MM/dd HH:mm:ss.FFF"),
                "TradeNum:", TradeNum, "Sec:", SecCode, "Class:", ClassCode, "Prc:", Price.ToString("F2"),
                "Amnt:", Quantity.ToString("F0"));
        }
    }
}
