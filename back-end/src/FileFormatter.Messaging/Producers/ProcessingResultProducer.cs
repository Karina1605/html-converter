using Confluent.Kafka;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Common;
using FileFormatter.Messaging.Dto;
using FileFormatter.Messaging.Options;
using Microsoft.Extensions.Options;

namespace FileFormatter.Messaging.Producers;

public class ProcessingResultProducer : IProcessingResultProducer
{
    private readonly IProducer<Guid, TaskProcessingResult> _producer;
    private readonly string _topic;

    public ProcessingResultProducer(IOptions<KafkaSettings> options)
    {
        var config = options.Value.BuildProducerConfiguration();

        _producer = new ProducerBuilder<Guid, TaskProcessingResult>(config).Build();
        _topic = options.Value.FileProcessedTopic;
    }

    public async Task OnFileRocessingFinished(TaskProcessingResult result)
    {
        await _producer.ProduceAsync(_topic, new Message<Guid, TaskProcessingResult>()
        {
            Key = result.RequestId,
            Value = result
        });
    }
}
