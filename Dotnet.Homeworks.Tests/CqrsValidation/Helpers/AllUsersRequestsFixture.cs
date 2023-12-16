using System.Reflection;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Tests.Cqrs.Helpers;

namespace Dotnet.Homeworks.Tests.CqrsValidation.Helpers;

[CollectionDefinition(nameof(AllUsersRequestsFixture))]
public class AllUsersRequestsFixture : IDisposable, ICollectionFixture<AllUsersRequestsFixture>
{
    private static Assembly AssemblyFeatures = Features.Helpers.AssemblyReference.Assembly;

    public AllUsersRequestsFixture()
    {
        if (!AllRequestsInAssemblyFixture() || !AllHandlersInAssemblyFixture())
            throw new ImplementInterfacesException(
                $"Not all UserManagement feature types implement required interfaces in {AssemblyFeatures.GetName().FullName} assembly"
            );
    }

    private bool AllRequestsInAssemblyFixture()
    {
        var interfaces = new List<Type>()
        {
            typeof(ICommand<>),
            typeof(ICommand),
            typeof(IQuery<>)
        };

        var types2 = AssemblyFeatures.GetTypes()
            .Where(x => x.Namespace.Contains("Users"))
            .Where(x => x.Name.EndsWith("Command") || x.Name.EndsWith("Query"));

        var types = types2
            .Select(x => interfaces.IntersectBy(x.GetInterfaces().Select(x => x.Name), type => type.Name));

        return types.All(x => x.Any());
    }

    private bool AllHandlersInAssemblyFixture()
    {
        var interfaces = new List<Type>()
        {
            typeof(IRequestHandler<,>),
            typeof(IRequestHandler<,>),
        };

        var types = AssemblyFeatures.GetTypes()
            .Where(x => x.Namespace.Contains("Users"))
            .Where(x => x.Name.EndsWith("Handler"))
            .Select(x => interfaces.IntersectBy(x.GetInterfaces().Select(x => x.Name), type => type.Name));

        return types.All(x => x.Any());
    }

    public void Dispose() =>
        GC.SuppressFinalize(this);
}
