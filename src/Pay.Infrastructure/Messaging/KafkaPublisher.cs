using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Pay.Infrastructure.Messaging
{
    public class KafkaPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaPublisher> _logger;

        public KafkaPublisher(ILogger<KafkaPublisher> logger)
        {
            _logger = logger;

            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task PublishAsync(string topic, string key, string payload)
        {
            _logger.LogInformation($"Publishing to Kafka: {topic} → {payload}");

            await _producer.ProduceAsync(
                topic,
                new Message<string, string> { Key = key, Value = payload }
            );

            _logger.LogInformation("Published successfully.");
        }
    }
}
