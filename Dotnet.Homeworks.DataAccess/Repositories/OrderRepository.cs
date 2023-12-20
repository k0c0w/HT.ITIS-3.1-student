using Dotnet.Homeworks.DataAccess.Repositories.Configuration;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using MongoDB.Driver;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _ordersDB;

    public OrderRepository(IMongoClient mongoClient, OrderRepositoryConfiguration configuration)
    {
        _ordersDB = mongoClient.GetDatabase(configuration.DatabaseName)
                               .GetCollection<Order>(configuration.CollectionName);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersFromUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userRealtedOrders = await _ordersDB.FindAsync(x => x.OrdererId == userId, cancellationToken: cancellationToken);

        return await userRealtedOrders
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderIdCursor = await _ordersDB.FindAsync(x => x.Id == orderId, cancellationToken: cancellationToken);

        return await orderIdCursor
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task DeleteOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return _ordersDB.FindOneAndDeleteAsync(x => x.Id == orderId, cancellationToken: cancellationToken);
    }

    public Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(s => s.Id, order.Id);
        var update = Builders<Order>.Update
            .Set(s => s.ProductsIds, order.ProductsIds);

        return _ordersDB.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public async Task<Guid> InsertOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _ordersDB.InsertOneAsync(order, cancellationToken: cancellationToken);

        return order.Id;
    }
}