using Confluent.Kafka;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Common;
using FileFormatter.Messaging.Dto;
using FileFormatter.Messaging.Options;
using Microsoft.Extensions.Options;

namespace FileFormatter.Messaging.Producers;

public class NewRequestProducer : INewRequestProducer
{
    private readonly IProducer<Guid, EnqueTaskMessage> _producer;
    private readonly string _topic;

    public NewRequestProducer(IOptions<KafkaSettings> options)
    {
        var config = options.Value.BuildProducerConfiguration();

        _producer = new ProducerBuilder<Guid, EnqueTaskMessage>(config).Build();
        _topic = options.Value.NewRequestTopic;
    }

    public async Task EnqueNewFilesBatch(Guid requestId, IReadOnlyCollection<string> filesNames, CancellationToken cancellationToken)
    {
        var tasks = new List<Task>();
        foreach (var file in filesNames)
        {
            tasks.Add(_producer.ProduceAsync(_topic, new Message<Guid, EnqueTaskMessage>
            {
                Key = requestId,
                Value = new EnqueTaskMessage(requestId, file)
            }, cancellationToken));
        }
        await Task.WhenAll(tasks);
    }
}
