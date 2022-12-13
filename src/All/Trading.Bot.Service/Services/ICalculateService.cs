using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;

namespace Trading.Bot.Service.Services
{
    public interface ICalculateService
    {
        decimal RecalculateSma(List<Bar> bars, int period);
        decimal RecalculateEma(List<Bar> bars, int period);
        (decimal? K, decimal? D) RecalculateStoch(List<Bar> bars, int kPeriod, int dPeriod);
    }
}
