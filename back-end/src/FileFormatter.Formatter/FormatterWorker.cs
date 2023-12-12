using Confluent.Kafka;
using FileFormatter.Formatter.Abstractions;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Common;
using FileFormatter.Messaging.Dto;
using FileFormatter.Messaging.Options;
using Microsoft.Extensions.Options;

namespace FileFormatter.Formatter;

public class FormatterWorker : IListener
{
    private readonly ISingleFileProcessor _singleFileProcessor;
    private readonly IConsumer<Guid, EnqueTaskMessage> _consumer;

    public FormatterWorker(ISingleFileProcessor singleFileProcessor, IOptions<KafkaSettings> settings)
    {
        _singleFileProcessor = singleFileProcessor;

        var config = settings.Value.BuildConsumerConfiguration();

        _consumer = new ConsumerBuilder<Guid, EnqueTaskMessage>(config).Build();
        _consumer.Subscribe(settings.Value.NewRequestTopic);
    }

    public Task Run(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var next = _consumer.Consume(cancellationToken);
                if (next != null)
                {
                    await _singleFileProcessor.ProcessNextAsync(
                        next.Message.Value.RequestId, 
                        next.Message.Value.FileName);
                    _consumer.Commit();
                }
            }
        });
        return Task.CompletedTask;
    }
}
