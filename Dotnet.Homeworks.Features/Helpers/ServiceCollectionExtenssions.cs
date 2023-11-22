using Dotnet.Homeworks.DataAccess.Repositories;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.PipelineBehaviors;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Features.Helpers;

public static class ServiceCollectionExtenssions
{
    public static void AddFeaturesDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IProductRepository, ProductRepository>();

        services.AddMediator(AssemblyReference.Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}
