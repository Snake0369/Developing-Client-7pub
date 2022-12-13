using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts
{
    public class Index
    {
        public Guid Id { get; set; }
        public string Ticker { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual List<BarIndexHistory> BarIndexHistories { get; set; } = new List<BarIndexHistory>();
    }
}
