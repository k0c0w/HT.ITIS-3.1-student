using Dotnet.Homeworks.Storage.API.Constants;
using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;
using Minio.DataModel.Args;

namespace Dotnet.Homeworks.Storage.API.Services;

public class StorageFactory : IStorageFactory
{
    private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    private readonly IMinioClient _client;

    public StorageFactory(IMinioClient client)
    {
        _client = client;
    }

    public async Task<IStorage<Image>> CreateImageStorageWithinBucketAsync(string bucketName)
    {
        IStorage<Image> storage = new ImageStorage(_client, bucketName);
        var tasks = new Task[2];

        await _semaphoreSlim.WaitAsync();
        try
        {
            tasks[0] = CreateBucketIfNotExistsAsync(Buckets.Pending);
            tasks[1] = bucketName == Buckets.Pending ? Task.CompletedTask : CreateBucketIfNotExistsAsync(bucketName);
            await Task.WhenAll(tasks);

            return storage;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task CreateBucketIfNotExistsAsync(string bucketName)
    {
        var bucketExistsArgs = new BucketExistsArgs()
               .WithBucket(bucketName);
        var bucketExists = await _client.BucketExistsAsync(bucketExistsArgs);

        if (bucketExists)
            return;

        var args = new MakeBucketArgs()
                .WithBucket(bucketName);

        await _client.MakeBucketAsync(args);
    }
}