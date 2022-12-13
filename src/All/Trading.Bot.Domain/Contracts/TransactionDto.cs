using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Brokerref { get; set; } = string.Empty;
        public string OrderNum { get; set; } = string.Empty;
        public int Status { get; set; }
        public string ResultMsg { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string TransId { get; set; } = string.Empty;
        public string Uid { get; set; } = string.Empty;
        public string SecCode { get; set; } = string.Empty;
        public string ClassCode { get; set; } = string.Empty;
        public DateTime? Updated { get; set; }
    }
}
