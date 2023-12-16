using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Domain.Exceptions;
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
            .ToListAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => _ctx.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task DeleteProductByGuidAsync(Guid id, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var entityToRemove = await GetByIdAsync(id, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return;

        if (entityToRemove == null)
            throw new EntityNotFoundException(id, typeof(Product));

        _ctx.Products.Remove(entityToRemove);

        if (cancellationToken.IsCancellationRequested)
            StopTrackingEntryById(id);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var previousEntry = await GetByIdAsync(product.Id, cancellationToken);
        if (previousEntry == null)
            throw new EntityNotFoundException(product.Id, typeof(Product));

        _ctx.Products.Update(product);

        if (cancellationToken.IsCancellationRequested)
            StopTrackingEntryById(product.Id);
    }

    public async Task<Guid> InsertProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _ctx.Products.AddAsync(product, cancellationToken);

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