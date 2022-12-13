using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.DataAccess.Services;
using Trading.Bot.Domain.Contracts;


namespace Trading.Bot.DataAccess.Repositories
{
    public class TradingFutRepository : TradingFutContextService, ITradingFutRepository
    {
        public TradingFutRepository(DbContextOptions<TradingFutContext> options) : base(options)
        {
        }

        public async Task AddBarDtoAsync(BarDto bar, CancellationToken cancellationToken = default)
        {
            await Insert(bar, cancellationToken);
        }

        public async Task AddOrderDtoAsync(OrderDto order, CancellationToken cancellationToken = default)
        {
            await Insert(order, cancellationToken);
        }

        public async Task AddTradeDtoAsync(TradeDto trade, CancellationToken cancellationToken = default)
        {
            await Insert(trade, cancellationToken);
        }

        public async Task AddTradeAllDtoAsync(TradeAllDto trade, CancellationToken cancellationToken = default)
        {
            await Insert(trade, cancellationToken);
        }

        public async Task AddTransactionDtoAsync(TransactionDto transaction, CancellationToken cancellationToken = default)
        {
            await Insert(transaction, cancellationToken);
        }

        public async Task AddOrdersDtoAsync(IEnumerable<OrderDto> orders, CancellationToken cancellationToken = default)
        {
            await Insert(orders, cancellationToken);
        }

        public async Task AddTradesDtoAsync(IEnumerable<TradeDto> trades, CancellationToken cancellationToken = default)
        {
            await Insert(trades, cancellationToken);
        }

        public async Task AddTradeAllsDtoAsync(IEnumerable<TradeAllDto> trades, CancellationToken cancellationToken = default)
        {
            await Insert(trades, cancellationToken);
        }

        public async Task AddTransactionsDtoAsync(IEnumerable<TransactionDto> transactions, CancellationToken cancellationToken = default)
        {
            await Insert(transactions, cancellationToken);
        }

        public async Task AddOrderAsync(Order order, CancellationToken cancellationToken = default)
        {
            await Insert(order, cancellationToken);
        }

        public async Task AddOrdersAsync(IEnumerable<Order> orders, CancellationToken cancellationToken = default)
        {
            await Insert(orders, cancellationToken);
        }

        public async Task AddTradeAsync(Trade trade, CancellationToken cancellationToken = default)
        {
            await Insert(trade, cancellationToken);
        }

        public async Task AddTradesAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken = default)
        {
            await Insert(trades, cancellationToken);
        }

        public async Task AddTradeHistoryAsync(TradeHistory trade, CancellationToken cancellationToken = default)
        {
            await Insert(trade, cancellationToken);
        }

        public async Task AddTradesHistoryAsync(IEnumerable<TradeHistory> trades, CancellationToken cancellationToken = default)
        {
            await Insert(trades, cancellationToken);
        }

        public async Task AddSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default)
        {
            await Insert(subposition, cancellationToken);
        }

        public async Task UpdateSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default)
        {
            await Update(subposition, cancellationToken);
        }

        public async Task AddSubpositionsAsync(IEnumerable<Subposition> subpositions, CancellationToken cancellationToken = default)
        {
            await Insert(subpositions, cancellationToken);
        }

        public async Task AddPositionAsync(Position position, CancellationToken cancellationToken = default)
        {
            await Insert(position, cancellationToken);
        }

        public async Task UpdatePositionAsync(Position position, CancellationToken cancellationToken = default)
        {
            await Update(position, cancellationToken);
        }

        public async Task AddPositionsAsync(IEnumerable<Position> positions, CancellationToken cancellationToken = default)
        {
            await Insert(positions, cancellationToken);
        }

        public async Task AddRenkoAsync(Renko renko, CancellationToken cancellationToken = default)
        {
            await Insert(renko, cancellationToken);
        }

        public async Task<List<Strategy>> GetStrategiesAsync(CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Strategies
                    .Include(p => p.Positions.OrderBy(x => x.Created))
                    .ThenInclude(sub => sub.Subpositions.OrderBy(x => x.Created))
                    .Include(e => e.Equities.OrderBy(x => x.Timestamp))
                    .OrderBy(p => p.Timestamp)
                    .ToListAsync(cancellationToken));
        }
        public async Task<Strategy?> GetStrategyAsync(Guid strategyId, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Strategies
                    .Include(p => p.Positions.OrderBy(x => x.Created))
                    .ThenInclude(sub => sub.Subpositions.OrderBy(x => x.Created))
                    .Include(e => e.Equities.OrderBy(x => x.Timestamp))
                    .OrderBy(p => p.Timestamp).FirstOrDefaultAsync(s => s.Id == strategyId, cancellationToken));
        }

        public async Task<Subposition?> GetOrdersAsync(Subposition subposition, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Subpositions
                    .FirstOrDefaultAsync(sub => sub.Id == subposition.Id, cancellationToken));
        }

        public async Task AddContractAsync(
            Contract contract,
            CancellationToken cancellationToken = default)
        {
            await Insert(contract, cancellationToken);
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

        public async Task AddEquitiesAsync(IEnumerable<Equity> equities, CancellationToken cancellationToken = default)
        {
            await Insert(equities, cancellationToken);
        }

        public async Task AddTradeAsync(
            TradeHistory trade,
            CancellationToken cancellationToken = default)
        {
            await Insert(trade, cancellationToken);
        }

        public async Task AddTradesAsync(
            IEnumerable<TradeHistory> trades,
            CancellationToken cancellationToken = default)
        {
            await Insert(trades, cancellationToken);
        }

        public async Task AddFileHistoryAsync(
            FileHistory file,
            CancellationToken cancellationToken = default)
        {
            file.LastModified = DateTime.Now;
            await Insert(file, cancellationToken);
        }

        public async Task AddFileArchiveAsync(
            FileArchive file,
            CancellationToken cancellationToken = default)
        {
            await Insert(file, cancellationToken);
        }

        public async Task<FileArchive?> GetFileArchiveAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                 await db.FileArchives
                    .Include(f => f.Files.OrderBy(x => x.LastModified))
                    .FirstOrDefaultAsync(f => f.Id == id, cancellationToken));
        }


        public async Task AddBarsAsync(IEnumerable<BarHistory> bars, CancellationToken cancellationToken = default)
        {
            await Insert(bars, cancellationToken);
        }

        public async Task AddBarAsync(BarHistory bar, CancellationToken cancellationToken = default)
        {
            await Insert(bar, cancellationToken);
        }

        public async Task<IEnumerable<BarHistory>> GetBarsAsync(Contract contract, int timeframe, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.BarHistories
                    .Where(bar => bar.ContractId == contract.Id && bar.Timeframe == timeframe)
                    .OrderBy(bar => bar.Timestamp)
                    .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<BarHistory>> GetBarsAsync(Contract contract, DateTime date, int timeframe, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.BarHistories
                    .Where(bar => bar.ContractId == contract.Id && bar.Timeframe == timeframe 
                        && bar.Timestamp.Date == date.Date)
                    .OrderBy(bar => bar.Timestamp)
                    .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<BarHistory>> GetBarsAsync(string baseTicker, int timeframe, CancellationToken cancellationToken = default)
        {
            var contracts = await GetContractsAsync(baseTicker);
            return await UseContextAsync(async db =>
                await db.BarHistories
                    .Where(bar => contracts.Any(contract => bar.ContractId == contract.Id) && bar.Timeframe == timeframe)
                    .OrderBy(bar => bar.Timestamp)
                    .ToListAsync(cancellationToken));
        }

        public async Task<BarHistory?> GetLastBarAsync(string baseTicker, int timeframe, CancellationToken cancellationToken = default)
        {
            var contracts = await GetContractsAsync(baseTicker);
            return await UseContextAsync(async db =>
                await db.BarHistories
                    .AsAsyncEnumerable()
                    .Where(bar => contracts.Any(contract => bar.ContractId == contract.Id) && bar.Timeframe == timeframe)
                    .OrderBy(bar => bar.Timestamp)
                    .LastOrDefaultAsync(cancellationToken));
        }
        public async Task DeleteBarsByDateAsync(Contract contract, int timeframe, DateTime date, CancellationToken cancellationToken = default)
        {
            var bars = await GetBarsAsync(contract, timeframe, cancellationToken);
            bars = bars.Where(x => x.Timestamp.Date == date.Date);
            if (bars.Any())
            {
                await Delete(bars);
            }
        }

        public async Task<TradeHistory?> GetFirstTradeAsync(string baseTicker, CancellationToken cancellationToken = default)
        {
            var contracts = (await GetContractsAsync(baseTicker)).ToList();
            if (contracts.Any())
            {
                return await UseContextAsync(async db =>
                    await db.TradesHistories
                        .AsAsyncEnumerable()
                        .Where(trade => contracts.Exists(contract => contract.Id == trade.ContractId))
                        .OrderBy(trade => trade.Timestamp)
                        .FirstOrDefaultAsync(cancellationToken));
            }
            return null;
        }

        public async Task<TradeHistory?> GetLastTradeAsync(string baseTicker, CancellationToken cancellationToken = default)
        {
            var contracts = await GetContractsAsync(baseTicker);
            return await UseContextAsync(async db =>
                await db.TradesHistories
                    .AsAsyncEnumerable()
                    .Where(trade => contracts.Any(contract => trade.ContractId == contract.Id))
                    .OrderBy(trade => trade.Timestamp)
                    .LastOrDefaultAsync(cancellationToken));
        }

        public async Task UpdateFileHistoryAsync(
            FileHistory fileHistory,
            CancellationToken cancellationToken = default)
        {
            await Update(fileHistory, cancellationToken);
        }

        public async Task<Contract?> GetContractAsync(string ticker, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Contracts
                    .FirstOrDefaultAsync(contract => contract.Ticker == ticker, cancellationToken));
        }

        public async Task<Contract?> GetContractAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Contracts
                    .FirstOrDefaultAsync(contract => contract.Id == id, cancellationToken));
        }

        public async Task<Contract?> GetCurrentContractAsync(DateTime date, string baseTicker, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Contracts
                    .Where(contract => contract.BaseTicker == baseTicker)
                    .OrderBy(contract => contract.LastTradingDate)
                    .FirstOrDefaultAsync(contract => contract.LastTradingDate >= date, cancellationToken));
        }

        public async Task<Contract?> GetContractByExpiredAsync(DateTime expired, string baseTicker, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Contracts
                    .Where(contract => contract.BaseTicker == baseTicker)
                    .FirstOrDefaultAsync(contract => contract.Expired.Date == expired.Date, cancellationToken));
        }

        public async Task<IEnumerable<Contract>> GetCurrentContractsAsync(DateTime date, CancellationToken cancellationToken = default)
        {
            var contracts = new ConcurrentBag<Contract>();
            var contractsAll = await UseContextAsync(async db =>
                await db.Contracts
                    .OrderBy(contract => contract.LastTradingDate)
                    .ToListAsync(cancellationToken));
            Parallel.ForEach(contractsAll.GroupBy(g => g.BaseTicker), contractByBaseTicker =>
            {
                var contract = contractByBaseTicker.OrderBy(c => c.LastTradingDate)
                    .FirstOrDefault(c => c.LastTradingDate >= date);
                if (contract is not null)
                {
                    contracts.Add(contract);
                }
            });
            return contracts;
        }

        public async Task<IEnumerable<Contract>> GetContractsAsync(string baseTicker, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Contracts
                    .Where(contract => contract.BaseTicker == baseTicker).ToListAsync(cancellationToken));    
        }
        
        public async Task<IEnumerable<Renko>> GetRenkosByContract(Contract contract, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Renkos
                    .Where(renko => renko.ContractId == contract.Id)
                    .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<Renko>> GetRenkosByContract(Contract contract, int timeframe, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Renkos
                    .Where(renko => renko.ContractId == contract.Id && renko.Timeframe == timeframe)
                    .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<Renko>> GetRenkosByContractByDate(Contract contract, int timeframe, DateTime date, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.Renkos
                    .Where(renko => renko.ContractId == contract.Id
                        && renko.Timeframe == timeframe && renko.Timestamp.Date == date.Date)
                    .ToListAsync(cancellationToken));
        }

        public async Task DeleteRenkosByDate(Contract contract, int timeframe, DateTime date, CancellationToken cancellationToken = default)
        {
            var renkos = await GetRenkosByContractByDate(contract, timeframe, date, cancellationToken);
            if (renkos.Any())
            {
                await Delete(renkos);
            }
        }


        public async Task<IEnumerable<TradeHistory>> GetTradesAsync(Contract contract, DateTime? date = null, CancellationToken cancellationToken = default)
        {
            if (date != null)
            {
                return await UseContextAsync(async db =>
                    await db.TradesHistories
                        .Where(trade => trade.ContractId == contract.Id && trade.Timestamp.Date == date.Value.Date)
                        .OrderBy(x => x.Id)
                        .ToListAsync(cancellationToken));
            }
            return await UseContextAsync(async db =>
                await db.TradesHistories
                    .Where(trade => trade.ContractId == contract.Id)
                    .OrderBy(x => x.Id)
                    .ToListAsync(cancellationToken));
        }


        public async Task DeleteTradesAsync(Contract contract, DateTime date, CancellationToken cancellationToken = default)
        {
            var trades  = await GetTradesAsync(contract, date, cancellationToken);
            if (trades.Any())
            {
                await Delete(trades);
            }
        }

        public async Task AddCommentMapAsync(CommentMap commentMap, CancellationToken cancellationToken = default)
        {
            await Insert(commentMap, cancellationToken);
        }

        public async Task<List<CommentMap>> GetCommentMapsAsync(CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.CommentMaps
                     .ToListAsync(cancellationToken));
        }

        public async Task AddOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken = default)
        {
            await Insert(orderItem, cancellationToken);
        }

        public async Task AddOrderChannelAsync(OrderChannel orderChannel, CancellationToken cancellationToken = default)
        {
            await Insert(orderChannel, cancellationToken);
        }

        public async Task UpdateOrderChannelAsync(OrderChannel orderChannel, CancellationToken cancellationToken = default)
        {
            await Update(orderChannel, cancellationToken);
        }

        public async Task<List<OrderChannel>> GetOrderChannelsAsync(CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.OrderChannels
                     .Include(x => x.Items)
                     .ThenInclude(x => x.Orders.OrderBy(x => x.Updated))
                     .Include(x => x.Items)
                     .ThenInclude(x => x.Trades.OrderBy(x => x.Created))
                     .ToListAsync(cancellationToken));
        }

        public async Task<OrderChannel?> GetOrderChannelAsync(OrderChannel orderChannel, CancellationToken cancellationToken = default)
        {
            return await UseContextAsync(async db =>
                await db.OrderChannels
                     .Include(x => x.Items)
                     .ThenInclude(x => x.Orders.OrderBy(x => x.Updated))
                     .Include(x => x.Items)
                     .ThenInclude(x => x.Trades.OrderBy(x => x.Created))
                     .FirstOrDefaultAsync(x => x.Id == orderChannel.Id, cancellationToken));
        }

    }
}
