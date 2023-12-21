using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Features.Users.Queries.GetUser;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions
{
    public partial class UserMapper : IUserMapper
    {
        public GetUserDto MapFromUser(User p1)
        {
            return p1 == null ? null : new GetUserDto(p1.Id, p1.Name, p1.Email);
        }
    }
}