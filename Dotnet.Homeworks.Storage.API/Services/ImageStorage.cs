using Dotnet.Homeworks.Shared.Dto;
using Dotnet.Homeworks.Storage.API.Constants;
using Dotnet.Homeworks.Storage.API.Dto.Internal;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Reactive.Linq;

namespace Dotnet.Homeworks.Storage.API.Services;

public class ImageStorage : IStorage<Image>
{
    private readonly string _targetBucket;
    private readonly IMinioClient _client;

    public ImageStorage(IMinioClient client, string targetBucket)
    {
        _targetBucket = targetBucket;
        _client = client;
    }

    public async Task<Result> PutItemAsync(Image item, CancellationToken cancellationToken = default)
    {
        var objectExistanceResult = await ObjectExitstsAsync(_targetBucket, item.FileName);
        if (objectExistanceResult.IsFailure)
            return objectExistanceResult;
        if (objectExistanceResult.Value)
            return new Result(false, error: $"Object with name \"{item.FileName}\" had been already created.");
        objectExistanceResult = await ObjectExitstsAsync(Buckets.Pending, item.FileName);
        if (objectExistanceResult.IsFailure)
            return objectExistanceResult;
        if (objectExistanceResult.Value)
            return new Result(false, error: $"Object with name \"{item.FileName}\" had been added to upload queue.");
        
        if (item.Metadata.TryGetValue(MetadataKeys.Destination, out var destinationBucket))
        {
            if (destinationBucket != _targetBucket)
                item.Metadata[MetadataKeys.Destination] = _targetBucket;
        }
        else 
            item.Metadata.Add(MetadataKeys.Destination, _targetBucket);

        var args = new PutObjectArgs()
            .WithBucket(Buckets.Pending)
            .WithObject(item.FileName)
            .WithObjectSize(item.Content.Length)
            .WithContentType(item.ContentType)
            .WithHeaders(item.Metadata)
            .WithStreamData(item.Content);

        try
        {
            await _client.PutObjectAsync(args, cancellationToken);
            return new Result(true);
        }
        catch (MinioException ex)
        {
            return new Result(false, ex.Message);
        }
    }

    public async Task<Image?> GetItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var stream = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(_targetBucket)
            .WithObject(itemName)
            .WithCallbackStream(s => s.CopyToAsync(stream, cancellationToken));

        try
        {
            var response = await _client.GetObjectAsync(args, cancellationToken);

            stream.Position = 0;
            return new Image(stream, response.ObjectName, response.ContentType, response.MetaData);
        }
        catch (BucketNotFoundException)
        {
            return null;
        }
        catch (ObjectNotFoundException)
        {
            return null;
        }
    }

    public async Task<Result> RemoveItemAsync(string itemName, CancellationToken cancellationToken = default)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(_targetBucket)
            .WithObject(itemName);

        try
        {
            await _client.RemoveObjectAsync(args, cancellationToken);

            return new Result(true);
        }
        catch (MinioException ex)
        {
            return new Result(false, error: ex.Message);
        }
    }

    public async Task<IEnumerable<string>> EnumerateItemNamesAsync(CancellationToken cancellationToken = default)
    {
        var args = new ListObjectsArgs()
            .WithBucket(_targetBucket);

        try
        {
            var objectsInfos = await _client.ListObjectsAsync(args, cancellationToken)
                .Select(x => x.Key)
                .ToArray();

            return objectsInfos;
        }
        catch (MinioException)
        {
            return Array.Empty<string>();
        }
    }

    public async Task<Result> CopyItemToBucketAsync(string itemName, string destinationBucketName,
        CancellationToken cancellationToken = default)
    {
        var thisBucketObjectExistanceResult = await ObjectExitstsAsync(_targetBucket, itemName);
        if (thisBucketObjectExistanceResult.IsFailure)
            return thisBucketObjectExistanceResult;
        if (!thisBucketObjectExistanceResult.Value)
            return new Result(false, $"Object \"{itemName}\" was not found.");
        var thatBucketObjectExistanceResult = await ObjectExitstsAsync(destinationBucketName, itemName);
        if (thatBucketObjectExistanceResult.IsFailure)
            return thisBucketObjectExistanceResult;
        if (thatBucketObjectExistanceResult.Value)
            return new Result(false, $"Object \"{itemName}\" alredy exists in {destinationBucketName} bucket.");

        var args = new CopyObjectArgs()
             .WithBucket(destinationBucketName)
             .WithObject(itemName)
             .WithCopyObjectSource(new CopySourceObjectArgs()
                 .WithBucket(_targetBucket)
                 .WithObject(itemName));
        try
        {
            await _client.CopyObjectAsync(args, cancellationToken);

            return new Result(true);
        }
        catch (MinioException ex)
        {
            return new Result(false, error: ex.Message);
        }
    }

    private async Task<Result<bool>> ObjectExitstsAsync(string bucket, string objectName)
    {
        var args = new StatObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName);

        try
        {
            var stats = await _client.StatObjectAsync(args);
            bool fileExists = !stats.ExtraHeaders.TryGetValue("X-Minio-Error-Desc", out string? value) || value != "\"The specified key does not exist.\"";
            return new Result<bool>(fileExists, true);
        }
        catch (ErrorResponseException)
        {
            return new Result<bool>(false, true);
        }
        catch (MinioException ex)
        {
            return new Result<bool>(default, false, error: ex.Message);
        }
    }
}