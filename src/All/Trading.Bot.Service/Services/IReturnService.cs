using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Domain.Contracts;

namespace Trading.Bot.Service.Services
{
    public interface IReturnService
    {
        decimal Return(decimal price, IEnumerable<Position> positions);
    }
}
