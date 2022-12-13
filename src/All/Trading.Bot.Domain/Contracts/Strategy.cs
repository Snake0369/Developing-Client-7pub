using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class Strategy
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public virtual List<Position> Positions { get; set; } = new List<Position>();
        public virtual List<Equity> Equities { get; set; } = new List<Equity>();
    }
}
