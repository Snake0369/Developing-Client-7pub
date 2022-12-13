using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Contracts;

namespace Trading.Access.Abstraction.Repositories
{
    public interface ITradingRepository
    {
        Task AddReturnAsync(ReturnIssue ret, CancellationToken cancellationToken = default);
        Task AddOrderAsync(Order order, CancellationToken cancellationToken = default);
        Task AddTradeAsync(Trade trade, CancellationToken cancellationToken = default);
        Task AddSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default);
        Task UpdateSubpositionAsync(Subposition subposition, CancellationToken cancellationToken = default);
        Task AddPositionAsync(Position position, CancellationToken cancellationToken = default);
        Task UpdatePositionAsync(Position position, CancellationToken cancellationToken = default);
        Task AddFuturePositionAsync(FuturePosition position, CancellationToken cancellationToken = default);
        Task<List<ReturnIssue>?> GetReturnsAsync(CancellationToken cancellationToken = default);
        Task<List<Order>?> GetOrdersAsync(CancellationToken cancellationToken = default);
        Task<List<Trade>?> GetTradesAsync(CancellationToken cancellationToken = default);
        Task<Strategy?> GetStrategyAsync(Guid strategyId, CancellationToken cancellationToken = default);
        Task<Strategy?> GetStrategyAllPositionsAsync(Guid strategyId, CancellationToken cancellationToken = default);
        Task AddStrategyAsync(
            Strategy strategy,
            CancellationToken cancellationToken = default);
        Task AddEquityAsync(Equity equity, CancellationToken cancellationToken = default);
    }
}
