using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _ctx;

    public ProductRepository(AppDbContext context)
    {
        _ctx = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return await _ctx
            .Products
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => _ctx.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task DeleteProductByGuidAsync(Guid id, CancellationToken cancellationToken)
    {
        var entityToRemove = await GetByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (entityToRemove == null || cancellationToken.IsCancellationRequested)
            return;

        _ctx.Products.Remove(entityToRemove);

        if (cancellationToken.IsCancellationRequested)
            StopTrackingEntryById(id);
    }

    public Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        if  (cancellationToken.IsCancellationRequested)
            return Task.CompletedTask;

        _ctx.Products.Update(product);

        if (cancellationToken.IsCancellationRequested)
            StopTrackingEntryById(product.Id);

        return Task.CompletedTask;
    }

    public async Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _ctx.Products.AddAsync(product, cancellationToken).ConfigureAwait(false);

        if (cancellationToken.IsCancellationRequested)
            StopTrackingEntryById(product.Id);

        return product.Id;
    }

    private void StopTrackingEntryById(Guid id)
    {
        var entry = _ctx.ChangeTracker.Entries<Product>().FirstOrDefault(x => x.Property(y => y.Id).CurrentValue == id);

        if (entry != null)
            entry.State = EntityState.Detached;
    }
}