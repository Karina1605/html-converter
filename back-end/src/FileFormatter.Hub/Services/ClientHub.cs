using Confluent.Kafka;
using FileFormatter.HubRunner.Abstractions;
using FileFormatter.HubRunner.Contracts.Communication;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Common;
using FileFormatter.Messaging.Dto;
using FileFormatter.Messaging.Options;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using ConnectionInfo = FileFormatter.HubRunner.Contracts.Communication.ConnectionInfo;

namespace FileFormatter.HubRunner.Services;

public class ClientHub : Hub, IClientHub, IListener
{
    private readonly ICache _cache;
    private readonly IConsumer<Guid, ProcessingFinalResult> _consumer;

    public ClientHub(
        ICache cache,
        IOptions<KafkaSettings> options)
    {
        _cache = cache;

        var config = options.Value.BuildConsumerConfiguration();
        _consumer = new ConsumerBuilder<Guid, ProcessingFinalResult>(config).Build();
        _consumer.Subscribe(options.Value.GeneratedLinksTopic);
    }

    public Task EstablishConnection(ConnectionInfo connectionInfo)
    {
        _cache.AddConnection(Guid.Parse(connectionInfo.RequestId), connectionInfo.ConnectionId);
        return Task.CompletedTask;
    }

    public async Task NotifyUpdate(FileUpdateInfo info, Guid requestId)
    {
        var connection = _cache.GetConnection(requestId);
        await this.Clients.Client(connection).SendAsync("updateInfo", info);
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
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
                    await NotifyUpdate(new FileUpdateInfo(
                        next.Message.Value.OriginalFileName, 
                        next.Message.Value.Status, 
                        next.Message.Value.DownloadLink), 
                    next.Message.Key);
                }
            }
        });
        return Task.CompletedTask;
    }
}
