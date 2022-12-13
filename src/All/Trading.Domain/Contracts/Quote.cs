using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts
{
    public class Quote
    {
        public DateTime Update { get; set; }
        public decimal Bid { get; set; }
        public decimal BidAmount { get; set; }
        public decimal Ask { get; set; }
        public decimal AskAmount { get; set; }
    }
}
