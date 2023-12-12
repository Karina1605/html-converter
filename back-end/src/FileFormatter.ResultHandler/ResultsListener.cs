using Confluent.Kafka;
using FileFormatter.DbIntegrator.Abstractions.Query;
using FileFormatter.LinksService.Constracts;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Common;
using FileFormatter.Messaging.Dto;
using FileFormatter.Messaging.Options;
using Microsoft.Extensions.Options;

namespace FileFormatter.ResultHandler;

public class ResultsListener : IListener
{
    private readonly IConsumer<Guid, TaskProcessingResult> _listener;

    private readonly IFileReadyToDownloadProducer _producer;
    private readonly ILinkBuilderClient _linkBuilderClient;
    private readonly IUpdateStateQuery _updateStatequery;

    public ResultsListener(
        IFileReadyToDownloadProducer producer,
        ILinkBuilderClient linkBuilderClient,
        IOptions<KafkaSettings> options,
        IUpdateStateQuery updateStateQuery)
    {
        _producer = producer;
        _linkBuilderClient = linkBuilderClient;
        _updateStatequery = updateStateQuery;

        var config = options.Value.BuildConsumerConfiguration();

        _listener = new ConsumerBuilder<Guid, TaskProcessingResult>(config).Build();
        _listener.Subscribe(options.Value.FileProcessedTopic);
    }

    public Task Run(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var next = _listener.Consume(cancellationToken);
                if (next != null)
                {
                    await _updateStatequery.UpdateFileState(
                        next.Message.Key,
                        next.Message.Value.OriginalFileName,
                        next.Message.Value.Result,
                        next.Message.Value.NewFileName,
                        cancellationToken);

                    if (next.Message.Value.Result == Common.Enums.FileProcessingStatus.Converted)
                    {
                        var link = await _linkBuilderClient.GenerateLinkAsync(new GenerateLinkRequest(
                            next.Message.Key,
                            next.Message.Value.NewFileName!));
                        await _producer.OnReadyToDownloadAsync(new ProcessingFinalResult(
                            next.Message.Key, 
                            next.Message.Value.OriginalFileName,
                            next.Message.Value.Result, 
                            link));
                    }

                    else if (next.Message.Value.Result == Common.Enums.FileProcessingStatus.ProcessingFailed)
                    {
                        await _producer.OnReadyToDownloadAsync(new ProcessingFinalResult(
                          next.Message.Key,
                          next.Message.Value.OriginalFileName,
                          next.Message.Value.Result,
                          null));
                    }
                }
            }

        });
        return Task.CompletedTask;
    }
}
