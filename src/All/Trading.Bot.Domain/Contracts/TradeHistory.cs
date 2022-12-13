using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class TradeHistory
    {
        public Guid Id { get; set; }
        public string TradeId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public Guid ContractId { get; set; }
        public decimal Price { get; set; }
        public decimal Volume { get; set; }
        public DateTime? Updated { get; set; }
        public virtual Contract? Contract { get; set; }
    }
}
