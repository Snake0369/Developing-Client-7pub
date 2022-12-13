using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class Equity
    {
        public Guid Id { get; set; }
        public Guid StrategyId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Instrument { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public decimal DrowDown { get; set; }
        public virtual Strategy? Strategy { get; set; }
    }
}
