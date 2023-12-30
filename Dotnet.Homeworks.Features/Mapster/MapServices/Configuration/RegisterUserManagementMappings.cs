using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Configuration;

public class RegisterUserManagementMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        {
            config.NewConfig<IQueryable<User>, GetAllUsersDto>()
                .Map(d => d.Users, s => s);
        }

        {
            config.NewConfig<User, Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers.GetUserDto>()
                .Map(d => d.Email, s => s.Email)
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.Name, s => s.Name);
        }
    }
}