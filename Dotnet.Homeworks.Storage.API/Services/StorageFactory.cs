using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;

namespace Dotnet.Homeworks.Storage.API.Services;

public class StorageFactory : IStorageFactory
{
    private readonly IMinioClient _client;

    public StorageFactory(IMinioClient client)
    {
        _client = client;
    }

    public Task<IStorage<Image>> CreateImageStorageWithinBucketAsync(string bucketName)
    {
        IStorage<Image> storage = new ImageStorage(_client, bucketName);

        return Task.FromResult(storage);
    }
}