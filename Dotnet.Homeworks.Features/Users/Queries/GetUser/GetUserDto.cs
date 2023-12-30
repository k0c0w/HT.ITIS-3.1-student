namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public record GetUserDto (
    Guid Id, 
    string Name, 
    string Email
    );