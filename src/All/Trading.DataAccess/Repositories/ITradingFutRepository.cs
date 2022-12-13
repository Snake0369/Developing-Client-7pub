using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;


namespace Trading.Bot.DataAccess.Repositories
{
    public interface ITradingFutRepository
    {
        Task AddBarDtoAsync(BarDto bar, CancellationToken cancellationToken = default);
        Task AddOrderDtoAsync(OrderDto order, CancellationToken cancellationToken = default);
        Task AddTradeDtoAsync(TradeDto trade, CancellationToken cancellationToken = default);
        Task AddTradeAllDtoAsync(TradeAllDto trade, CancellationToken cancellationToken = default);
        Task AddTransactionDtoAsync(TransactionDto transaction, CancellationToken cancellationToken = default);
        Task AddOrdersDtoAsync(IEnumerable<OrderDto> orders, CancellationToken cancellationToken = default);
        Task AddTradesDtoAsync(IEnumerable<TradeDto> trades, CancellationToken cancellationToken = default);
        Task AddTradeAllsDtoAsync(IEnumerable<TradeAllDto> trades, CancellationToken cancellationToken = default);
        Task AddTradeHistoryAsync(TradeHistory trade, CancellationToken cancellationToken = default);
        Task AddTradesHistoryAsync(IEnumerable<TradeHistory> trades, CancellationToken cancellationToken = default);
        Task AddTransactionsDtoAsync(IEnumerable<TransactionDto> transactions, CancellationToken cancellationToken = default);
        Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task AddOrdersAsync(IEnumerable<Order> orders, CancellationToken cancellationToken = default);
        Task AddTradeAsync(Trade trade, CancellationToken cancellationToken = default);
        Task AddTradesAsync(IEnumerable<Trade> trades, CancellationToken cancellationToken = default);
        Task AddSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default);
        Task AddSubpositionsAsync(IEnumerable<Subposition> subpositions, CancellationToken cancellationToken = default);
        Task UpdateSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default);
        Task AddPositionAsync(Position position, CancellationToken cancellationToken = default);
        Task UpdatePositionAsync(Position position, CancellationToken cancellationToken = default);
        Task AddPositionsAsync(IEnumerable<Position> positions, CancellationToken cancellationToken = default);
        Task<List<Strategy>> GetStrategiesAsync(CancellationToken cancellationToken = default);
        Task<Strategy?> GetStrategyAsync(Guid strategyId, CancellationToken cancellationToken = default);
        Task<Subposition?> GetOrdersAsync(Subposition subposition, CancellationToken cancellationToken = default);
        Task AddContractAsync(
            Contract contract,
            CancellationToken cancellationToken = default);
        Task AddStrategyAsync(
            Strategy strategy,
            CancellationToken cancellationToken = default);
        Task AddEquityAsync(Equity equity, CancellationToken cancellationToken = default);
        Task AddEquitiesAsync(IEnumerable<Equity> equities, CancellationToken cancellationToken = default);
        Task AddTradeAsync(
            TradeHistory trade,
            CancellationToken cancellationToken = default);
        Task AddTradesAsync(
            IEnumerable<TradeHistory> trades,
            CancellationToken cancellationToken = default);
        Task AddFileHistoryAsync(
            FileHistory file,
            CancellationToken cancellationToken = default);
        Task AddFileArchiveAsync(
            FileArchive file,
            CancellationToken cancellationToken = default);
        Task<FileArchive?> GetFileArchiveAsync(Guid id,
            CancellationToken cancellationToken = default);
        Task AddBarAsync(BarHistory bar, CancellationToken cancellationToken = default);
        Task AddBarsAsync(IEnumerable<BarHistory> bars, CancellationToken cancellationToken = default);
        Task<IEnumerable<BarHistory>> GetBarsAsync(Contract contract, int timeframe, CancellationToken cancellationToken = default);
        Task<IEnumerable<BarHistory>> GetBarsAsync(Contract contract, DateTime date, int timeframe, CancellationToken cancellationToken = default);
        Task<IEnumerable<BarHistory>> GetBarsAsync(string baseTicker, int timeframe, CancellationToken cancellationToken = default);
        Task<BarHistory?> GetLastBarAsync(string baseTicker, int timeframe, CancellationToken cancellationToken = default);
        Task<TradeHistory?> GetFirstTradeAsync(string baseTicker, CancellationToken cancellationToken = default);
        Task<TradeHistory?> GetLastTradeAsync(string baseTicker, CancellationToken cancellationToken = default);
        Task UpdateFileHistoryAsync(
                    FileHistory fileHistory,
                    CancellationToken cancellationToken = default);
        Task<Contract?> GetContractAsync(string ticker, CancellationToken cancellationToken = default);
        Task<Contract?> GetContractAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Contract?> GetCurrentContractAsync(DateTime date, string baseTicker, CancellationToken cancellationToken = default);
        Task<Contract?> GetContractByExpiredAsync(DateTime expired, string baseTicker, CancellationToken cancellationToken = default);
        Task<IEnumerable<Contract>> GetCurrentContractsAsync(DateTime date, CancellationToken cancellationToken = default);
        Task<IEnumerable<Contract>> GetContractsAsync(string baseTicker, CancellationToken cancellationToken = default);
        Task<IEnumerable<TradeHistory>> GetTradesAsync(Contract contract, DateTime? date = null, CancellationToken cancellationToken = default);
        Task DeleteTradesAsync(Contract contract, DateTime date, CancellationToken cancellationToken = default);
        Task DeleteBarsByDateAsync(Contract contract, int timeframe, DateTime date, CancellationToken cancellationToken = default);
        Task<IEnumerable<Renko>> GetRenkosByContractByDate(Contract contract, int timeframe, DateTime date, CancellationToken cancellationToken = default);
        Task DeleteRenkosByDate(Contract contract, int timeframe, DateTime date, CancellationToken cancellationToken = default);
        Task AddRenkoAsync(Renko renko, CancellationToken cancellationToken = default);
        Task<IEnumerable<Renko>> GetRenkosByContract(Contract contract, int timeframe, CancellationToken cancellationToken = default);
        Task<IEnumerable<Renko>> GetRenkosByContract(Contract contract, CancellationToken cancellationToken = default);
        Task AddCommentMapAsync(CommentMap commentMap, CancellationToken cancellationToken = default);
        Task<List<CommentMap>> GetCommentMapsAsync(CancellationToken cancellationToken = default);
        Task AddOrderItemAsync(OrderItem orderItem, CancellationToken cancellationToken = default);
        Task AddOrderChannelAsync(OrderChannel orderChannel, CancellationToken cancellationToken = default);
        Task<OrderChannel?> GetOrderChannelAsync(OrderChannel orderChannel, CancellationToken cancellationToken = default);
        Task<List<OrderChannel>> GetOrderChannelsAsync(CancellationToken cancellationToken = default);
        Task UpdateOrderChannelAsync(OrderChannel orderChannel, CancellationToken cancellationToken = default);
    }
}
