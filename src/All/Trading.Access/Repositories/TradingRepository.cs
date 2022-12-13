using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Access.Abstraction.Repositories;
using Trading.Core.Static;
using Trading.DataAccess;
using Trading.DataAccess.Services;
using Trading.Domain.Contracts;
using Trading.Domain.Contracts.Enums;

namespace Trading.DataAccess.Repositories
{
    public class TradingRepository : TradingContextService, ITradingRepository
    {
        public TradingRepository(DbContextOptions<TradingContext> options) : base(options)
        {
        }

        public async Task AddReturnAsync(ReturnIssue ret, CancellationToken cancellationToken = default)
        {
            await Insert(ret, cancellationToken);
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await Insert(order, cancellationToken);
        }

        public async Task AddTradeAsync(Trade trade, CancellationToken cancellationToken = default)
        {
            await Insert(trade, cancellationToken);
        }

        public async Task AddSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default)
        {
            await Insert(subposition, cancellationToken);
        }

        public async Task UpdateSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default)
        {
            await Update(subposition, cancellationToken);
        }

        public async Task AddPositionAsync(Position position, CancellationToken cancellationToken = default)
        {
            await Insert(position, cancellationToken);
        }

        public async Task UpdatePositionAsync(Position position, CancellationToken cancellationToken = default)
        {
            await Update(position, cancellationToken);
        }

        public async Task AddFuturePositionAsync(FuturePosition position, CancellationToken cancellationToken = default)
        {
            await Insert(position, cancellationToken);
        }

        public async Task<List<ReturnIssue>?> GetReturnsAsync(CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Returns
                    .ToListAsync(cancellationToken));
        }

        public async Task<List<Order>?> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Orders
                    .ToListAsync(cancellationToken));
        }

        public async Task<List<Trade>?> GetTradesAsync(CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Trades
                    .ToListAsync(cancellationToken));
        }

        public async Task<Strategy?> GetStrategyAsync(Guid strategyId, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Strategies
                    .Include(p => p.Positions.Where(x => x.BaseAsset == BotSettings.BaseAsset && x.QuoteAsset == BotSettings.QuoteAsset))
                    .ThenInclude(s => s.Subpositions)
                    .ThenInclude(o => o.Orders)
                    .Include(p => p.Positions.Where(x => x.BaseAsset == BotSettings.BaseAsset && x.QuoteAsset == BotSettings.QuoteAsset))
                    .ThenInclude(s => s.Subpositions)
                    .ThenInclude(t => t.Trades)
                    .Include(e => e.Equities)
                    .FirstOrDefaultAsync(st => st.Id == strategyId, cancellationToken));
        }

        public async Task<Strategy?> GetStrategyAllPositionsAsync(Guid strategyId, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Strategies
                    .Include(p => p.Positions)
                    .ThenInclude(s => s.Subpositions)
                    .ThenInclude(o => o.Orders)
                    .Include(p => p.Positions)
                    .ThenInclude(s => s.Subpositions)
                    .ThenInclude(t => t.Trades)
                    .Include(e => e.Equities)
                    .FirstOrDefaultAsync(st => st.Id == strategyId, cancellationToken));
        }

        public async Task AddStrategyAsync(
            Strategy strategy,
            CancellationToken cancellationToken = default)
        {
            await Insert(strategy, cancellationToken);
        }

        public async Task AddEquityAsync(Equity equity, CancellationToken cancellationToken = default)
        {
            await Insert(equity, cancellationToken);
        }
    }
}
