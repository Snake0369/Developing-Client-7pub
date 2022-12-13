using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Domain.Contracts.Enums
{
    public enum OrderState
    {
        None = 0,
        Create = 1,
        CreateProcessed = 2,
        Created = 3,
        Cancel = 4,
        CancelProcessed = 5,
        Cancelled = 6,
        Complated = 7,
        Error = 8,
        WaitCancellRequest = 9,
        Trading = 10
    }
}
