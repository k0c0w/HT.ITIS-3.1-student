using Dotnet.Homeworks.Domain.Abstractions.Repositories;

namespace Dotnet.Homeworks.Infrastructure.UnitOfWork;

public interface IUnitOfWork
{
    public IProductRepository ProductRepository { get; }

    Task SaveChangesAsync(CancellationToken token);
}