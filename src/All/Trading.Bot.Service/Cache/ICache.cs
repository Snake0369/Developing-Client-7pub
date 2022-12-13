using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;

namespace Trading.Bot.Service.Cache
{
    public interface ICache
    {
        Task<List<TradeHistory>> GetTradesAsync(Contract contract, DateTime date);
    }
}
