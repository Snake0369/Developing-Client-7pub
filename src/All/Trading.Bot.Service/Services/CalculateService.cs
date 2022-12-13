using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;
using Trading.Bot.TransferData.Services;
using Trading.Core.Extensions;

namespace Trading.Bot.Service.Services
{
    public class CalculateService : ICalculateService
    {
        public CalculateService()
        {
        }

        public decimal RecalculateSma(List<Bar> bars, int period)
        {
            if (bars.Count < period + 1)
            {
                throw new ArgumentException("Период расчета параметра должен быть меньше или равен длине входящего массива");
            }
            var sma = bars.TakeLast(period).Select(b => b.Open).ToList().Sma();
            return sma ?? throw new InvalidDataException("Не удалось рассчитать параметр");
        }

        public decimal RecalculateEma(List<Bar> bars, int period)
        {
            if (bars.Count < period + 1)
            {
                throw new ArgumentException("Период расчета параметра должен быть меньше или равен длине входящего массива");
            }
            var ema = bars.TakeLast(period).Select(b => b.Open).ToList().Ema();
            return ema ?? throw new InvalidDataException("Не удалось рассчитать параметр");
        }

        public (decimal? K, decimal? D) RecalculateStoch(List<Bar> bars, int kPeriod, int dPeriod)
        {
            if (bars.Count < kPeriod + dPeriod + 1)
            {
                throw new ArgumentException("Период расчета параметра должен быть меньше или равен длине входящего массива");
            }
            var stoch = bars.TakeLast(kPeriod + dPeriod + 1).SkipLast(1).ToList().Stoch(kPeriod, dPeriod);
            if (stoch.K != null && stoch.D != null)
            {
                return stoch; 
            }
            throw new InvalidDataException("Не удалось рассчитать параметр");
        }
    }
}
