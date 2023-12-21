using Dotnet.Homeworks.Storage.API.Constants;
using Dotnet.Homeworks.Storage.API.Dto.Internal;

namespace Dotnet.Homeworks.Storage.API.Services;

public class PendingObjectsProcessor : BackgroundService
{
    private readonly IStorage<Image> _pendingImageStorage;
    private readonly ILogger _logger;

    public PendingObjectsProcessor(IStorageFactory storageFactory, ILogger<PendingObjectsProcessor> logger)
    {
        _pendingImageStorage = storageFactory.CreateImageStorageWithinBucketAsync(Buckets.Pending).GetAwaiter().GetResult();
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                foreach (var itemName in await _pendingImageStorage.EnumerateItemNamesAsync(stoppingToken))
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    var imageS3File = await _pendingImageStorage.GetItemAsync(itemName, stoppingToken);

                    if (imageS3File is not null)
                    {
                        if (imageS3File.Metadata.TryGetValue(MetadataKeys.Destination, out var destinationBucket))
                            await _pendingImageStorage.CopyItemToBucketAsync(itemName, destinationBucket, stoppingToken);
                         
                        var removementResult = await _pendingImageStorage.RemoveItemAsync(itemName, stoppingToken);
                        if (removementResult.IsFailure)
                            _logger.LogInformation(removementResult.Error);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            await Task.Delay(PendingObjectProcessor.Period, stoppingToken);
        }
    }
}