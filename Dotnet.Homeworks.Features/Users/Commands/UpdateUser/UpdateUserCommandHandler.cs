using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : CqrsDecorator<UpdateUserCommand, Result>, ICommandHandler<UpdateUserCommand>
{ 
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEnumerable<IValidator<UpdateUserCommand>> validators, IPermissionCheck<IClientRequest>? permissionCheck) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var pipelineResult = await base.Handle(request, cancellationToken);

        if (pipelineResult.IsFailure)
            return pipelineResult;

        if (cancellationToken.IsCancellationRequested)
            return new Result(false);

        await _userRepository.UpdateUserAsync(request.User, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return new Result(false);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return new Result(false);

        return new Result(true);
    }
}