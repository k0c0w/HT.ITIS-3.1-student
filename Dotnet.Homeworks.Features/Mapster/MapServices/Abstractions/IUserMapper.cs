using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;

[Mapper]
public interface IUserMapper
{
    GetUserDto MapFromUser(User user);
}