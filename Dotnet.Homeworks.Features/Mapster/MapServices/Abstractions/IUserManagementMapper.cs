using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;

[Mapper]
public interface IUserManagementMapper
{
    GetAllUsersDto MapFromUsers(IQueryable<User> users);
}