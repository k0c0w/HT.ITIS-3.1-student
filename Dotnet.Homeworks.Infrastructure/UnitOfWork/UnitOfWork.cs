using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.DataAccess.Repositories;

namespace Dotnet.Homeworks.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;

    public IProductRepository ProductRepository { get; }

    public UnitOfWork(AppDbContext context)
    {
        ProductRepository = new ProductRepository(context);
        _ctx = context;
    }

    public Task SaveChangesAsync(CancellationToken token)
        => _ctx.SaveChangesAsync(token);
}