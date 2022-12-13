using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class TradeDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Brokerref { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TradeNum { get; set; } = string.Empty;
        public string OrderNum { get; set; } = string.Empty;
        public uint Flags { get; set; }
        public string Account { get; set; } = string.Empty;
        public string TransId { get; set; } = string.Empty;
        public string Uid { get; set; } = string.Empty;
        public string SecCode { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public decimal Price { get; set; } = decimal.Zero;
        public decimal Quantity { get; set; } = decimal.Zero;
        public decimal Commission { get; set; } = decimal.Zero;
        public DateTime? Updated { get; set; }
    }
}
