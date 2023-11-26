using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _ctx;

    public UserRepository(AppDbContext context)
    {
        _ctx = context;
    }

    public async Task<IQueryable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
       return (await _ctx.Users.ToArrayAsync(cancellationToken)).AsQueryable();
    }

    public Task<User?> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        return _ctx.Users.FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);
    }

    public async Task DeleteUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        var user = await GetUserByGuidAsync(guid, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return;

        if (user == null)
            throw new InvalidOperationException($"User was not found: {guid}.");

        _ctx.Users.Remove(user);
    }

    public Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        if (!cancellationToken.IsCancellationRequested)
            _ctx.Users.Update(user);

        return Task.CompletedTask;
    }

    public async Task<Guid> InsertUserAsync(User user, CancellationToken cancellationToken)
    {
        var emailAlreadyExists = await _ctx.Users.AnyAsync(x => x.Email == user.Email);

        if (emailAlreadyExists)
            throw new InvalidOperationException("User with such email already exists.");

        await _ctx.AddAsync(user, cancellationToken);

        return user.Id;
    }
}