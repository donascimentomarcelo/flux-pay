using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pay.Infrastructure.Database;
using Pay.Infrastructure.Messaging;

namespace Pay.OutboxWorker
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutboxProcessor> _logger;

        private readonly KafkaPublisher _publisher;

        public OutboxProcessor(IServiceProvider serviceProvider, ILogger<OutboxProcessor> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _publisher = serviceProvider.GetRequiredService<KafkaPublisher>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<FluxPayDbContext>();

                    var pending = await db
                        .OutboxMessages.Where(x => x.ProcessedAt == null)
                        .OrderBy(x => x.CreatedAt)
                        .Take(10)
                        .ToListAsync(stoppingToken);

                    foreach (var msg in pending)
                    {
                        try
                        {
                            await _publisher.PublishAsync(
                                topic: "payments",
                                key: msg.Id.ToString(),
                                payload: msg.Payload
                            );

                            Console.WriteLine($"Publishing: {msg.Type} → {msg.Payload}");

                            msg.ProcessedAt = DateTime.UtcNow;
                            await db.SaveChangesAsync(stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            msg.Error = ex.ToString();
                            await db.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing outbox.");
                }

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
