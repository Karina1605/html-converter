namespace FileFormatter.StoreIntegrator.Abstractions;

public interface IStoreService
{
    Task AddFileToStorage(Guid bucketId, string fileName, Stream stream);

    Task AddBucket(Guid bucketId, Dictionary<string, Stream> files);

    Task DropBucket(Guid bucketId);

    Task<string> GenerateTemporaryLink(Guid bucketId, string fileName);
}
