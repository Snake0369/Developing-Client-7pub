using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class Renko
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Timeframe { get; set; }
        public Guid ContractId { get; set; }
        public decimal HighBorder { get; set; }
        public decimal LowBorder { get; set; }
        public virtual Contract? Contract { get; set; }
    }
}
