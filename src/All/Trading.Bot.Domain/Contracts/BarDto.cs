using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class BarDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string SecCode { get; set; } = default!;
        public string ClassCode { get; set; } = default!;
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp2 { get; set; }
        public DateTime Updated { get; set; }
    }
}
