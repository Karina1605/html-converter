using FileFormatter.LinksService.Constracts;
using FileFormatter.LinksService.Options;
using Refit;

namespace FileFormatter.LinksService.Extensions;

public static class LinkBuilderExtensions
{
    public static IServiceCollection AddLinkBuilderClient(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(LinkServiceOptions.SectionName) as LinkServiceOptions;

        if (section == null)
        {
            throw new Exception("Url not found");
        }

        var serviceAddress = section.Url;

        services.AddRefitClient<ILinkBuilderClient>()
            .ConfigureHttpClient(x => x.BaseAddress = new Uri(serviceAddress));

        return services;
    }
}
