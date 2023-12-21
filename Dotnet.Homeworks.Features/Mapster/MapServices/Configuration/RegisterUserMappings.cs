using Dotnet.Homeworks.Domain.Entities;
using Mapster;

namespace Dotnet.Homeworks.Features.Mapster.MapServices.Configuration;

public class RegisterUserMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        {
            config.NewConfig<User, Dotnet.Homeworks.Features.Users.Queries.GetUser.GetUserDto>()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.Name, s => s.Name)
                .Map(d => d.Email, s => s.Email);
        }
    }
}