using Confluent.Kafka;
using Confluent.Kafka.Admin;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Common;
using FileFormatter.Messaging.Dto;
using FileFormatter.Messaging.Options;
using Microsoft.Extensions.Options;

namespace FileFormatter.Messaging.Producers;

public class FileReadyToDownloadProducer : IFileReadyToDownloadProducer
{
    private readonly IProducer<Guid, ProcessingFinalResult> _producer;
    private readonly string _topic;

    public FileReadyToDownloadProducer(IOptions<KafkaSettings> settings)
    {
        var config = settings.Value.BuildProducerConfiguration();

        _producer = new ProducerBuilder<Guid, ProcessingFinalResult>(config).Build();
        _topic = settings.Value.GeneratedLinksTopic;
    }

    public async Task OnReadyToDownloadAsync(ProcessingFinalResult linkBuildingResult)
    {
        await _producer.ProduceAsync(_topic, new Message<Guid, ProcessingFinalResult>()
        {
            Key = linkBuildingResult.RequestId,
            Value = linkBuildingResult
        });
    }
}
