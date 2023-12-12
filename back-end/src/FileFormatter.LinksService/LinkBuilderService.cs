using FileFormatter.LinksService.Abstractions;
using FileFormatter.StoreIntegrator.Abstractions;

namespace FileFormatter.LinksService;

public class LinkBuilderService : ILinkBuilderService
{
    private readonly IStoreService _storeService;

    public LinkBuilderService(IStoreService storeService)
    {
        _storeService = storeService;
    }

    public Task<string> GenerateDownloadLink(Guid bucketId, string s3FileName)
    {
        return _storeService.GenerateTemporaryLink(bucketId, s3FileName);
    }
}
