using FileFormatter.StoreIntegrator.Abstractions;
using FileFormatter.StoreIntegrator.Settings;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using System.IO;

namespace FileFormatter.StoreIntegrator;

public class StoreService : IStoreService
{
    private readonly IMinioClient _minioClient;

     public StoreService(IMinioClient minioClient) 
    {
        _minioClient = minioClient;
        /*_minioClient = new MinioClient()
            .WithEndpoint("localhost:9000")
            .WithCredentials("admin", "supersecret");*/
    }

    public async Task AddBucket(Guid bucketId, Dictionary<string, Stream> files)
    {
        await CreateBucketIfNotExists(bucketId.ToString());
        var tasksList = new List<Task>();
        foreach (var file in files)
        {
            tasksList.Add(new Task(async x =>
            {
                var keyValue = x as KeyValuePair<string, Stream>?;
                var args = new PutObjectArgs()
                    .WithBucket(bucketId.ToString())
                    .WithObject(keyValue!.Value.Key)
                    .WithContentType("application/octet-stream")
                    .WithFileName(keyValue.Value.Key)
                    .WithStreamData(keyValue.Value.Value);
                await _minioClient.PutObjectAsync(args).ConfigureAwait(false);
            }, file));
        }
        await Task.WhenAll(tasksList.ToArray());
    }

    public async Task AddFileToStorage(Guid bucketId, string fileName, Stream stream)
    {
        try
        {
            Console.WriteLine("Running example for API: PutObjectAsync with FileName");
            var args = new PutObjectArgs()
            .WithBucket(bucketId.ToString())
                .WithObject(fileName)
                .WithContentType("application/octet-stream")
                .WithFileName(fileName)
                .WithStreamData(stream);
            await _minioClient.PutObjectAsync(args).ConfigureAwait(false);

        }
        catch (Exception e)
        {
            Console.WriteLine($"[Bucket]  Exception: {e}");
        }
    }

    public async Task DropBucket(Guid bucketId)
    {
        try
        {
            await _minioClient.RemoveBucketAsync(
                new RemoveBucketArgs()
                    .WithBucket(bucketId.ToString())
            ).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Bucket]  Exception: {e}");
        }
    }

    public async Task<string> GenerateTemporaryLink(Guid bucketId, string fileName)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(bucketId.ToString())
            .WithObject(fileName)
            .WithExpiry(3600 * 24);
        return await _minioClient.PresignedGetObjectAsync(args);
    }

    private async Task CreateBucketIfNotExists(string bucketId)
    {
        try
        {
            var args = new BucketExistsArgs()
            .WithBucket(bucketId);
            var found = await _minioClient.BucketExistsAsync(args);
            if (!found)
            {
                await _minioClient.MakeBucketAsync(
                new MakeBucketArgs()
                .WithBucket(bucketId)
                ).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        }
    }
}
