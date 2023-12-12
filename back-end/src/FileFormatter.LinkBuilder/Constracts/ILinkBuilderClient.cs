using Refit;

namespace FileFormatter.LinkBuilder.Constracts;

public interface ILinkBuilderClient 
{
    private const string BasePath = "/generator";

    [Get($"{BasePath}/generate-link")]
    Task<string> GenerateLinkAsync([Query] GenerateLinkRequest request);
}
