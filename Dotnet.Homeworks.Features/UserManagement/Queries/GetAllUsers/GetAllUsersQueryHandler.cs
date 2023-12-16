using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.UserManagement.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, GetAllUsersDto>
{
    private readonly IUserRepository _userRepository;

    public GetAllUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _userRepository.GetUsersAsync(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
                return new Result<GetAllUsersDto>(null, false);

            return new Result<GetAllUsersDto>(new GetAllUsersDto(users.Select(x => new GetUserDto(x.Id, x.Name, x.Email))), true);
        }
        catch (Exception ex)
        {
            return new Result<GetAllUsersDto>(null, false, ex.Message);
        }
    }
}