using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Domain.Contracts.Enums;

namespace Trading.Domain.Contracts.Extensions
{
    public static class SideExtensions
    {
        public static Side Invert(this Side side) => side == Side.Buy ? Side.Sell : Side.Buy;
    }
}
