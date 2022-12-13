using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts
{
    public class Symbol
    {
        public Guid Id { get; set; }
        public Enums.Exchange Exchange { get; set; }
        public string BaseAsset { get; set; }
        public string QuoteAsset { get; set; }
        public string Name { get; set; }
        public decimal SizeFilter { get; set; }
        public decimal PriceFilter { get; set; }
        public decimal MinLot { get; set; }
        public decimal MaxLot { get; set; }
    }
}
