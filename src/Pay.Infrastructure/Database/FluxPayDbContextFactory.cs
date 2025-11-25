using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pay.Infrastructure.Database
{
    public class FluxPayDbContextFactory : IDesignTimeDbContextFactory<FluxPayDbContext>
    {
        public FluxPayDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                "Host=localhost;Port=5432;Database=fluxpay;Username=fluxpay;Password=fluxpay123";

            var optionsBuilder = new DbContextOptionsBuilder<FluxPayDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new FluxPayDbContext(optionsBuilder.Options);
        }
    }
}
