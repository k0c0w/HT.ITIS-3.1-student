using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;
using System.Diagnostics;

namespace Dotnet.Homeworks.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : CqrsDecorator<GetUserQuery, Result<GetUserDto>>, IQueryHandler<GetUserQuery, GetUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMapper _userMapper;

    public GetUserQueryHandler(IUserRepository userRepository, IEnumerable<IValidator<GetUserQuery>> validators, 
        IPermissionCheck<IClientRequest>? permissionCheck, IUserMapper userMapper) 
        : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _userMapper = userMapper;
    }

    public override async Task<Result<GetUserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var pipelineResult = await base.Handle(request, cancellationToken);
        if (pipelineResult.IsFailure)
            return new Result<GetUserDto>(default, false, pipelineResult.Error);

        var user = await _userRepository.GetUserByGuidAsync(request.Guid, cancellationToken)!;

        if (user is null)
            return new Result<GetUserDto>(default, false, $"User not found: {request.Guid}");

        Debug.Assert(user != null);
        return  new Result<GetUserDto>(_userMapper.MapFromUser(user!), true);
    }
}