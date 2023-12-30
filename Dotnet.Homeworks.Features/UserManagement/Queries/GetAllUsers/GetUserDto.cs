namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public record GetUserDto (
    Guid Id, 
    string Name, 
    string Email
    );