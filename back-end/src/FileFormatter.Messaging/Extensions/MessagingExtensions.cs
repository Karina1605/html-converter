using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Options;
using FileFormatter.Messaging.Producers;
using Microsoft.Extensions.DependencyInjection;

namespace FileFormatter.Messaging.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddKafkaMessaging(this IServiceCollection services)
    {
        services.AddOptions().ConfigureOptions<KafkaSettings>();

        services
            .AddScoped<IFileReadyToDownloadProducer, FileReadyToDownloadProducer>()
            .AddScoped<INewRequestProducer, NewRequestProducer>()
            .AddScoped<IProcessingResultProducer, ProcessingResultProducer>();

        return services;
    }
}
