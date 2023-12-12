namespace FileFormatter.LinksService.Abstractions;

public interface ILinkBuilderService
{
    Task<string> GenerateDownloadLink(Guid bucketId, string s3FileName);
}
