using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Core.Settings;
using Trading.Bot.DataAccess.Repositories;
using Trading.Bot.Domain.Contracts;
using Trading.Bot.Domain.Contracts.Enums;
using Trading.Bot.Domain.Extensions;
using Trading.Bot.Notification.Telegram.Services;
using Trading.Bot.TransferData.Command;
using Trading.Bot.TransferData.Services;

namespace Trading.Bot.Service.Services
{
    public class PositionsService : IPositionsService
    {
        private readonly ILogger _logger = Log.ForContext<PositionsService>();
        private readonly IOrderCommand _orderCommand;
        private readonly ITradingFutRepository _tradingFutRepository;
        private readonly ISendController _sendController;
        private readonly List<StrategySettings> _settings;
        private readonly ITelegramMessageService _telegramMessageService;
        private Strategy? _strategy = null;

        public PositionsService(IOrderCommand orderCommand,
            ITradingFutRepository tradingFutHistoryRepository,
            ISendController sendController,
            List<StrategySettings> settings,
            ITelegramMessageService telegramMessageService)
        {
            _orderCommand = orderCommand;
            _tradingFutRepository = tradingFutHistoryRepository;
            _sendController = sendController;
            _settings = settings;
            _telegramMessageService = telegramMessageService;
        }

        public async Task<Strategy?> LoadPositions(Strategy strategy)
        {
            var strategyEx = await _tradingFutRepository.GetStrategyAsync(strategy.Id);
            if (strategyEx == null)
            {
                await _tradingFutRepository.AddStrategyAsync(strategy);
                _strategy = strategy;
                return _strategy;
            }
            else if (strategyEx != null)
            {
                _strategy = strategyEx;
                return _strategy;
            }
            return null;
        }

        public async Task OpenPosition(OrderChannel channel, Contract contract, TimeSpan timeframe, Side side, Bar bar, decimal capital, decimal amount, OrderType orderType)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            _logger.Information("Open position ([Contract:{@contract} Timeframe:{@timeframe}])", contract.Ticker, timeframe);
            await _telegramMessageService.ToConcatedString($"Open position ([Contract:{contract.Ticker} Timeframe:{timeframe}])");
            var position = Position.Create(_strategy, contract, timeframe, side, capital, bar.Timestamp);
            await position.Open(channel, amount, bar.Open, orderType, bar.Timestamp, CreatePosition, CreateOrder, UpdateSubpositionState);
            _strategy.Positions.Add(position);
        }

        public async Task OpenPosition(OrderChannel channel, Contract contract, TimeSpan timeframe, Side side, decimal price, DateTime timestamp, decimal capital, decimal amount, OrderType orderType)
        {
            var bar = new Bar
            {
                Timestamp = timestamp,
                Open = price
            };
            await OpenPosition(channel, contract, timeframe, side, bar, capital, amount, orderType);
        }

        public async Task ClosePosition(Contract contract, TimeSpan timeframe, Bar bar)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            var position = GetLastPosition(contract, timeframe);
            if (position != null && position.State == State.Open)
            {
                _logger.Information("Close position ([Contract:{@contract} Timeframe:{@timeframe}])", contract.Ticker, timeframe);
                await _telegramMessageService.ToConcatedString($"Close position ([Contract:{contract.Ticker} Timeframe:{timeframe}])");
                await position.Close(bar.Open, bar.Timestamp, UpdatePosition, CreateOrder, UpdateSubpositionState);
            }
        }

        public async Task ClosePosition(Contract contract, TimeSpan timeframe, decimal price, DateTime timestamp)
        {
            var bar = new Bar
            {
                Timestamp = timestamp,
                Open = price
            };
            await ClosePosition(contract, timeframe, bar);
        }

        public async Task ChangePosition(Contract contract, TimeSpan timeframe, Bar bar, decimal amount)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            var position = GetLastPosition(contract, timeframe);
            if (position != null && position.State == State.Open)
            {
                var amountPosition = position.GetAmount();
                var amountQuantity = position.GetQuantity();
                var quantity = (int)(position.Amount * amount / bar.Open);
                if (Math.Abs(amountPosition) > amount)
                {
                    await position.Update(position.Side.Invert(), Math.Abs(amountPosition) - amount,
                        Math.Abs(amountQuantity) - quantity, bar.Open, bar.Timestamp, null, CreateOrder, UpdateSubpositionState);
                }
                else if (Math.Abs(amountPosition) < amount)
                {
                    await position.Update(position.Side, amount - Math.Abs(amountPosition),
                        quantity - Math.Abs(amountQuantity), bar.Open, bar.Timestamp, null, CreateOrder, UpdateSubpositionState);
                }
            }
        }

        public async Task ChangeSubpositionState(Subposition subposition, SubpositionState state)
        {
            await subposition.SetState(state, UpdateSubpositionState);
        }

        public State GetLastPositionStatus(Contract contract, TimeSpan timeframe)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            var positions = _strategy.Positions
                .Where(x => x.ContractId == contract.Id && x.Timeframe == (int)timeframe.TotalMinutes);
            if (positions.Any())
            {
                return positions.Last().State;
            }
            return State.Closed;
        }

        public Side? GetLastPositionSide(Contract contract, TimeSpan timeframe)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            var positions = _strategy.Positions
                .Where(x => x.ContractId == contract.Id && x.Timeframe == (int)timeframe.TotalMinutes);
            if (positions.Any())
            {
                return positions.Last().Side;
            }
            return null;
        }

        public decimal? GetLastPositionEntryPrice(Contract contract, TimeSpan timeframe)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            var positions = _strategy.Positions
                .Where(x => x.ContractId == contract.Id && x.Timeframe == (int)timeframe.TotalMinutes);
            if (positions.Any())
            {
                return positions.Last().GetEntryPrice();
            }
            return null;
        }

        public Position? GetLastPosition(Contract contract, TimeSpan timeframe)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            var positions = _strategy.Positions
                .Where(x => x.ContractId == contract.Id && x.Timeframe == (int)timeframe.TotalMinutes);
            if (positions.Any())
            {
                return positions.Last();
            }
            return null;
        }

        public List<Position> GetPositions(Contract contract, TimeSpan timeframe)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            return _strategy.Positions
                .Where(x => x.ContractId == contract.Id && x.Timeframe == (int)timeframe.TotalMinutes)
                .ToList();
        }

        public List<Position> GetPositions(Contract contract)
        {
            if (_strategy == null)
            {
                throw new ArgumentNullException(nameof(_strategy));
            }
            return _strategy.Positions
                .Where(x => x.ContractId == contract.Id)
                .ToList();
        }

        public Func<Subposition, Task> GetUpdateSubposition() => UpdateSubpositionState;

        private async Task CreateOrder(Subposition subposition)
        {
            await _tradingFutRepository.AddSubpositionAsync(subposition);
            _orderCommand.CreateOrder(subposition);
        }

        private async Task CreatePosition(Position position)
        {
            await _tradingFutRepository.AddPositionAsync(position);
        }

        private async Task UpdatePosition(Position position)
        {
            await _tradingFutRepository.UpdatePositionAsync(position);
        }

        private async Task UpdateSubpositionState(Subposition subposition)
        {
            if (subposition.Position != null && subposition.Position.Contract != null)
            {
                if (subposition.State == SubpositionState.NotImplemented)
                {
                    _sendController.Send($"Subposition;New;{subposition.Position.Contract.Ticker};{subposition.Id}");
                }
                else if (subposition.State == SubpositionState.Implemented || subposition.State == SubpositionState.Cancelled)
                {
                    _sendController.Send($"Subposition;Complated;{subposition.Position.Contract.Ticker};{subposition.Id}");
                    if (subposition.State == SubpositionState.Implemented)
                    {
                        if (subposition.Position.State == State.Open)
                        {
                            var settings = _settings.FirstOrDefault(x => x.Contract == subposition.Position.Contract.BaseTicker);
                            if (settings != null)
                            {
                                if (settings.TimeframeSettings.TryGetValue(new TimeSpan(0, subposition.Position.Timeframe, 0), out var timeframeSettings))
                                {
                                    var entryPrice = subposition.Position.GetEntryPrice();
                                    var minBorder = entryPrice - (subposition.Position.Side == Side.Buy
                                        ? timeframeSettings.StopLossLong : timeframeSettings.TakeProfitShort);
                                    var maxBorder = entryPrice + (subposition.Position.Side == Side.Buy
                                        ? timeframeSettings.TakeProfitLong : timeframeSettings.StopLossShort);
                                    _sendController.Send($"Position;New;{subposition.Position.Contract.Ticker};" +
                                        $"{subposition.Position.Id};{minBorder:F2};{maxBorder:F2}");
                                }
                            }
                        }
                        else if (subposition.Position.State == State.Closed)
                        {
                            _sendController.Send($"Position;Closed;{subposition.Position.Contract.Ticker};" +
                                $"{subposition.Position.Id}");
                        }
                    }
                }
            }
            await _tradingFutRepository.UpdateSubpositionAsync(subposition);
        }
    }
}
