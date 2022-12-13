using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts
{
    public class ReturnIssue
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime FirstPositionsTimestamp { get; set; }
        public DateTime LastPositionsTimestamp { get; set; }
        public decimal Current { get; set; }
        public decimal Maximum { get; set; }
        public bool IsTrade { get; set; }
    }
}
