using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : CqrsDecorator<DeleteUserCommand, Result>, ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEnumerable<IValidator<DeleteUserCommand>> validators, IPermissionCheck<IClientRequest>? permissionCheck) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var pipelineResult = await base.Handle(request, cancellationToken);

        if (pipelineResult.IsFailure)
            return new Result(false, pipelineResult.Error);

        try
        {
            await _userRepository.DeleteUserByGuidAsync(request.Guid, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new Result(true);
        }
        catch
        {
            return new Result(false);
        }
    }
}