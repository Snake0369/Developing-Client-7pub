using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Domain.Contracts.Enums
{
    public enum OrderStatus
    {
        Undefined = 0,
        New = 1,
        PartiallyFilled = 2,
        Filled = 3,
        Rejected = 4,
        Pending = 5,
        Canceled = 6,
        Expired = 7
    }
}
