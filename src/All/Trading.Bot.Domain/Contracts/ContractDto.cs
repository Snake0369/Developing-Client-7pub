using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class ContractDto
    {
        public string Market { get; set; } = string.Empty;
        public string BaseTicker { get; set; } = string.Empty;
        public string Ticker { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Expired { get; set; }
        public DateTime DateLastTrade { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
