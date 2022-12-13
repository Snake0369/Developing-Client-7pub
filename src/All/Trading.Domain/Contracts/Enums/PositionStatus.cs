using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts.Enums
{
    public enum PositionStatus
    {
        Normal,
        Liquidation,
        AutoDeleverage
    }
}
