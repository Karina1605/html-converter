using Confluent.Kafka;
using FileFormatter.Messaging.Options;

namespace FileFormatter.Messaging.Common;

public static class ConfigurationBuilder
{
    public static ProducerConfig BuildProducerConfiguration(this KafkaSettings settings)
    {
        return new ProducerConfig()
        {
            BootstrapServers = settings.Broker,
            RequestTimeoutMs = 5000,
            MessageTimeoutMs = 5000,
        };
    }

    public static ConsumerConfig BuildConsumerConfiguration(this KafkaSettings settings)
    {
        return new ConsumerConfig()
        {
            GroupId = settings.GroupId,
            BootstrapServers = settings.Broker,
            AutoOffsetReset = AutoOffsetReset.Latest
        };
    }
}
