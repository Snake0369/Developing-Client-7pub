using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Bot.Core.Context;

namespace Trading.Bot.DataAccess.Services
{
    public abstract class TradingFutContextService : DbContextService<TradingFutContext>
    {
        protected TradingFutContextService(DbContextOptions<TradingFutContext> options)
            : base(options)
        {
        }

        public sealed override TradingFutContext CreateContext(
            DbContextOptions<TradingFutContext> options) =>
            new TradingFutContext(options);
    }
}
