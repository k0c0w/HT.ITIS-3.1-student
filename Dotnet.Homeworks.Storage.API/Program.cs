using Dotnet.Homeworks.Storage.API.Endpoints;
using Dotnet.Homeworks.Storage.API.Services;
using Dotnet.Homeworks.Storage.API.ServicesExtensions;
using MinioConfig = Dotnet.Homeworks.Storage.API.Configuration.MinioConfig;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var minioConfig = new MinioConfig();
var minioConfigSection = configuration.GetSection(nameof(MinioConfig));
minioConfigSection.Bind(minioConfig);


services.Configure<MinioConfig>(minioConfigSection);
services.AddMinioClient(minioConfig);
services.AddSingleton<IStorageFactory, StorageFactory>();
services.AddHostedService<PendingObjectsProcessor>();
services.AddLogging();

var app = builder.Build();

app.MapProductsEndpoints();

app.Run();