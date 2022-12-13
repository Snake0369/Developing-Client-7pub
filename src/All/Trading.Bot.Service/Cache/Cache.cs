using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.DataAccess.Repositories;
using Trading.Bot.Domain.Contracts;

namespace Trading.Bot.Service.Cache
{
    public class Cache : ICache
    {
        private readonly ConcurrentDictionary<(Guid id, DateTime dt), List<TradeHistory>> _cache;
        private readonly ITradingFutRepository _repository;


        public Cache(ITradingFutRepository repository)
        {
            _repository = repository;
            _cache = new ConcurrentDictionary<(Guid id, DateTime dt), List<TradeHistory>>();
        }

        public async Task<List<TradeHistory>> GetTradesAsync(Contract contract, DateTime date)
        {
            List<TradeHistory>? trades;
            if (!_cache.TryGetValue((contract.Id, contract.LastTradingDate), out trades))
            {
                trades = (await _repository.GetTradesAsync(contract)).ToList();
                _cache.TryAdd((contract.Id, contract.LastTradingDate), trades);
            }
            if (_cache.Keys.Count > 25)
            {
                var key = _cache.Keys
                    .OrderBy(x => x.dt)
                    .First(x => x.id != contract.Id);
                _cache.TryRemove(key, out _);
            }
            return trades.Where(x => x.Timestamp.Date == date.Date).ToList();
        }
    }
}
