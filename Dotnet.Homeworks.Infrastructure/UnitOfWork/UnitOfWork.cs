using Dotnet.Homeworks.Data.DatabaseContext;

namespace Dotnet.Homeworks.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _ctx;

    public UnitOfWork(AppDbContext context)
    {
        _ctx = context;
    }

    public Task SaveChangesAsync(CancellationToken token)
        => _ctx.SaveChangesAsync(token);
}