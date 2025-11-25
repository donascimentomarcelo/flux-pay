using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pay.Infrastructure.Database;
using Pay.Infrastructure.Messaging;
using Pay.OutboxWorker;

Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        (context, services) =>
        {
            var connection = context.Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<FluxPayDbContext>(options => options.UseNpgsql(connection));

            services.AddHostedService<OutboxProcessor>();
            services.AddSingleton<KafkaPublisher>();
        }
    )
    .Build()
    .Run();
