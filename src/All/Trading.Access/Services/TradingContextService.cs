using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Core.Context;

namespace Trading.DataAccess.Services
{
    public abstract class TradingContextService : DbContextService<TradingContext>
    {
        protected TradingContextService(DbContextOptions<TradingContext> options)
            : base(options)
        {
        }

        public sealed override TradingContext CreateContext(
            DbContextOptions<TradingContext> options) =>
            new TradingContext(options);
    }
}
