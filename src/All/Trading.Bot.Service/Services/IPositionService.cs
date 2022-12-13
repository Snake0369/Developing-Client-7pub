using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;
using Trading.Bot.Domain.Contracts.Enums;

namespace Trading.Bot.Service.Services
{
    public interface IPositionsService
    {
        Task<Strategy?> LoadPositions(Strategy strategy);
        Task OpenPosition(OrderChannel channel, Contract contract, TimeSpan timeframe, Side side, Bar bar, decimal capital, decimal amount, OrderType orderType);
        Task OpenPosition(OrderChannel channel, Contract contract, TimeSpan timeframe, Side side, decimal price, DateTime timestamp, decimal capital, decimal amount, OrderType orderType);
        Task ClosePosition(Contract contract, TimeSpan timeframe, Bar bar);
        Task ClosePosition(Contract contract, TimeSpan timeframe, decimal price, DateTime timestamp);
        Task ChangePosition(Contract contract, TimeSpan timeframe, Bar bar, decimal amount);
        Task ChangeSubpositionState(Subposition subposition, SubpositionState state);
        State GetLastPositionStatus(Contract contract, TimeSpan timeframe);
        Side? GetLastPositionSide(Contract contract, TimeSpan timeframe);
        Position? GetLastPosition(Contract contract, TimeSpan timeframe);
        decimal? GetLastPositionEntryPrice(Contract contract, TimeSpan timeframe);
        List<Position> GetPositions(Contract contract, TimeSpan timeframe);
        List<Position> GetPositions(Contract contract);
        Func<Subposition, Task> GetUpdateSubposition();
    }
}
