using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;

namespace Trading.Bot.Service.Services
{
    public class ReturnService : IReturnService
    {
        public ReturnService()
        {
        }

        public decimal Return(decimal price, IEnumerable<Position> positions)
        {
            if (positions != null)
            {
                return positions
                    .Sum(s => s.GetProfit(price));
            }
            throw new ArgumentNullException(nameof(positions));
        }
    }
}
