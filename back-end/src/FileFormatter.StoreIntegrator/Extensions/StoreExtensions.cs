using FileFormatter.StoreIntegrator.Abstractions;
using FileFormatter.StoreIntegrator.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileFormatter.StoreIntegrator.Extensions;

public static class StoreExtensions
{
    public static IServiceCollection AddMinioStore(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<ClientSettings>();
        services.AddTransient<IStoreService, StoreService>();

        return services;
    }
}
