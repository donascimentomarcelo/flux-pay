using System.Text.Json;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pay.Domain.Entity;
using Pay.Domain.Strategies;
using Pay.Infrastructure.Database;
using Pay.Infrastructure.Messaging;

namespace Pay.PaymentProcessorWorker
{
    public class PaymentConsumerWorker(
        ILogger<PaymentConsumerWorker> logger,
        IServiceProvider serviceProvider,
        KafkaPublisher publisher
    ) : BackgroundService
    {
        private readonly ILogger<PaymentConsumerWorker> _logger = logger;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly KafkaPublisher _publisher = publisher;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "fluxpay-payment-processor",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true,
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe("payments");

            _logger.LogInformation(
                "PaymentProcessorWorker started. Listening to Kafka topic 'payments'."
            );

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken);

                    _logger.LogInformation(
                        $"[Kafka Consumer] Received event: {result.Message.Value}"
                    );

                    // TODO Refatorar e testar
                    var evt = JsonSerializer.Deserialize<Payment>(result.Message.Value);

                    var strategy = new PaymentStrategyFactory().GetStrategy(evt.Method);

                    var response = await strategy.ProcessAsync(evt);

                    _logger.LogInformation(
                        $"[PaymentProcessorWorker] Payment processed. Success: {response.Success}, Message: {response.Message}"
                    );

                    using var scope = _serviceProvider.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<FluxPayDbContext>();

                    var payment = await db.Payments.FirstOrDefaultAsync(x => x.Id == evt.Id);

                    if (payment != null)
                    {
                        payment.Status = response.Success
                            ? Domain.Enums.PaymentStatus.Approved
                            : Domain.Enums.PaymentStatus.Denied;
                        payment.UpdatedAt = DateTime.UtcNow;
                        await db.SaveChangesAsync(stoppingToken);
                    }

                    var processedEvent = new
                    {
                        paymentId = evt.Id,
                        status = payment.Status.ToString(),
                        message = response.Message,
                    };

                    await _publisher.PublishAsync(
                        "payments-processed",
                        evt.Id.ToString(),
                        JsonSerializer.Serialize(processedEvent)
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming Kafka message");
                }
            }
        }
    }
}
