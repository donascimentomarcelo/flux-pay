using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pay.PaymentProcessorWorker
{
    public class PaymentConsumerWorker(ILogger<PaymentConsumerWorker> logger) : BackgroundService
    {
        private readonly ILogger<PaymentConsumerWorker> _logger = logger;

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

                    // TODO processar strategy de pagamento aqui
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error consuming Kafka message");
                }
            }
        }
    }
}
