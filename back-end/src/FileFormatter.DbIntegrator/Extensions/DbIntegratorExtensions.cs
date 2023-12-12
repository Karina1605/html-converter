using FileFormatter.DbIntegrator.Abstractions.Commands;
using FileFormatter.DbIntegrator.Abstractions.Query;
using FileFormatter.DbIntegrator.Commands;
using FileFormatter.DbIntegrator.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileFormatter.DbIntegrator.Extensions;

public static class DbIntegratorExtensions
{
    public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions()
            .ConfigureOptions<DbConnectionSettings>();
        services
            .AddTransient<IAddSessionQuery, AddSessionQuery>()
            .AddTransient<IDropAndReturnFinishedSessionsQuery, DropAndReturnFinishedSessionsQuery>()
            .AddTransient<IInitializeTableQuery, InitializeTableQuery>()
            .AddTransient<IUpdateStateQuery, UpdateStateQuery>();

        return services;
    }
}
