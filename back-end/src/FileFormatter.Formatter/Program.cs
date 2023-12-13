using FileFormatter.Formatter;
using FileFormatter.LinksService.Extensions;
using FileFormatter.Messaging.Abstractions;
using FileFormatter.Messaging.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var hostBuilder = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json");

        var config = configuration.Build();

        services.AddKafkaMessaging();
        services.AddLinkBuilderClient(config);
        services.AddSingleton<IListener, FormatterWorker>();

    }).Build();
hostBuilder.Start();
var listener = hostBuilder.Services.GetRequiredService<IListener>();
await listener.Run(CancellationToken.None);
