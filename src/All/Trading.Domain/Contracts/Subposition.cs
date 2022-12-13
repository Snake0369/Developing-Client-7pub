using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Core.Extensions;
using Trading.Domain.Contracts.Enums;
using Trading.Domain.Contracts.Extensions;
using Trading.Domain.Services;

namespace Trading.Domain.Contracts
{
    public class Subposition
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid PositionId { get; set; }

        [NotMapped]
        public Subposition? LinkSubposition { get; set; } = null;
        public Guid? LinkSubpositionId { get; set; } = null;
        public SubpositionDirect SubpositionDirect { get; set; }
        public SubpositionType SubpositionType { get; set; }
        public string BaseAsset { get; set; } = default!;
        public string QuoteAsset { get; set; } = default!;
        public string Symbol { get; set; } = default!;
        public Side Side { get; set; }
        public decimal Amount { get; set; }
        public decimal? Price { get; set; }
        public SubpositionState State { get; set; }
        public string ClientOrderId { get; set; } = string.Empty;
        public virtual List<Order> Orders { get; set; } = new List<Order>();
        public virtual List<Trade> Trades { get; set; } = new List<Trade>();
        public virtual Position? Position { get; set; }

        [NotMapped]
        public Func<Subposition, Task>? Function;

        public static Subposition Add(Position position, SubpositionType type, Side side, string baseAsset, string quoteAsset, string symbol, decimal amount, Func<Subposition, Task>? function = null, DateTime? date = null, Subposition? link = null)
        {
            var generator = GeneratorClientOrders.GetGenerator();
            var clientOrderId = generator.GetClientOrder();
            return new Subposition
            {
                Id = Guid.NewGuid(),
                Timestamp = date == null ? DateTime.UtcNow : date.Value,
                PositionId = position.Id,
                BaseAsset = baseAsset,
                QuoteAsset = quoteAsset,
                Symbol = symbol,
                LinkSubposition = link,
                LinkSubpositionId = link?.Id,
                SubpositionDirect = SubpositionDirect.Direct,
                SubpositionType = type,
                Side = side,
                Amount = amount,
                State = SubpositionState.NotImplemented,
                ClientOrderId = clientOrderId,
                Position = position,
                Function = function
            };
        }

        public async Task Execute()
        {
            if (Function != null)
            {
                await Function(this);
            }
        }

        public Subposition CreateOppositSubposition(Func<Subposition, Task<string?>>? function = null, DateTime? date = null, Subposition? link = null)
        {
            var generator = GeneratorClientOrders.GetGenerator();
            var clientOrderId = generator.GetClientOrder();
            return new Subposition
            {
                Id = Guid.NewGuid(),
                Timestamp = date == null ? DateTime.UtcNow : date.Value,
                PositionId = PositionId,
                SubpositionType = SubpositionType,
                BaseAsset = BaseAsset,
                QuoteAsset = QuoteAsset,
                Symbol = Symbol,
                LinkSubposition = link,
                LinkSubpositionId = link?.Id,
                SubpositionDirect = SubpositionDirect.Opposit,
                Side = Side.Invert(),
                Amount = Amount,
                State = SubpositionState.NotImplemented,
                ClientOrderId = clientOrderId,
                Position = Position,
                Function = function
            };
        }

        public void AddTrade(
            string tradeId,
            string orderId,
            DateTime date,
            decimal price,
            decimal amount,
            decimal fee,
            string feeAsset,
            string? clientOrderId = null)
        {
            AddTrade(new Trade
            {
                Id = Guid.NewGuid(),
                TradeId = tradeId,
                Timestamp = date,
                SubpositionId = Id,
                OrderId = orderId,
                BaseAsset = BaseAsset,
                QuoteAsset = QuoteAsset,
                Symbol = Symbol,
                Price = price,
                Amount = amount,
                Fee = fee,
                FeeAsset = feeAsset,
                ClientOrderId = clientOrderId,
                Subposition = this
            });
        }

        public void AddTrade(Trade trade)
        {
            Trades.Add(trade);
        }

        public void AddOrder(
            string orderId,
            DateTime date,
            Side side,
            string type,
            decimal price,
            decimal amount,
            decimal amountRemaining,
            bool isMargin,
            OrderStatus status,
            string? clientOrderId = null)
        {
            AddOrder(new Order
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Timestamp = date,
                SubpositionId = Id,
                BaseAsset = BaseAsset,
                QuoteAsset = QuoteAsset,
                Symbol = Symbol,
                Side = side,
                Price = price,
                Amount = amount,
                Type = type,
                ClientOrderId = clientOrderId,
                AmountRemaining = amountRemaining,
                Status = status,
                IsMargin = isMargin,
                Subposition = this
            });
        }
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        public decimal GetCostUsdt(decimal rate = 1, decimal rateOfFee = 1, CancellationToken cancellationToken = default)
        {
            if (QuoteAsset.ToUpperInvariant() == "USDT")
            {
                rate = 1;
            }
            var result = Trades.Sum(x => x.Price * x.Amount * rate);
            var commission = Trades
                    .Sum(x => x.Fee * rateOfFee);
            return Side == Side.Buy ? -(result + commission) : result - commission;
        }

        public decimal? GetCost()
        {
            if (Trades.Any())
            {
                var cost = Trades.Where(x => x.TradeId != "-1").Sum(x => x.Price * x.Amount);
                return Side == Side.Buy ? -cost : cost;
            }
            return null;
        }

        public ValueTask<decimal> GetFee(Func<string?, decimal, Task<decimal>> returnFee)
        {
            return Trades.ToAsyncEnumerable().SumAwaitAsync(async x => await x.GetFee(returnFee));
        }

        public decimal GetFee(string feeAsset)
        {
            return Trades
                .Where(x => x.FeeAsset != null && x.FeeAsset.IsAssetEqual(feeAsset))
                .Sum(x => x.Fee);
        }

        public decimal? GetQuantity()
        {
            if (Trades.Any())
            {
                var result = Trades.Sum(x => x.Amount);
                return Side == Side.Sell ? -result : result;
            }
            return null;
        }
    }
}
