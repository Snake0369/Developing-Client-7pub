using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.DataAccess
{
    public class ContextFactory : IDesignTimeDbContextFactory<TradingFutContext>
    {
        public TradingFutContext CreateDbContext(string[] args)
        {
            var assembly = GetType().Assembly;

            var optionsBuilder = new DbContextOptionsBuilder<TradingFutContext>()
                .UseNpgsql("UserID=postgres;Password=Barracuda0369;Host=localhost;Port=5432;Database=postgres;Pooling=true;MinPoolSize=1;MaxPoolSize=20;CommandTimeout=600;");

            return new TradingFutContext(optionsBuilder.Options);
        }
    }
}
