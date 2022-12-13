using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.DataAccess
{
    public class ContextFactory : IDesignTimeDbContextFactory<TradingContext>
    {
        public TradingContext CreateDbContext(string[] args)
        {
            var assembly = GetType().Assembly;

            var optionsBuilder = new DbContextOptionsBuilder<TradingContext>()
                .UseNpgsql("UserID=trading;Password=232387;Host=109.230.162.73;Port=9698;Database=postgres;Pooling=true;MinPoolSize=1;MaxPoolSize=20;CommandTimeout=600;");
            return new TradingContext(optionsBuilder.Options);
        }
    }
}
