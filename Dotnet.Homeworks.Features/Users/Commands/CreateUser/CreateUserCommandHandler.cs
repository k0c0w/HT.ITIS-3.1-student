using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Decorators;
using Dotnet.Homeworks.Features.Users.Commands.CreateUser.Services;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : CqrsDecorator<CreateUserCommand, Result<CreateUserDto>>, ICommandHandler<CreateUserCommand, CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegistrationService _registrationService;

    public CreateUserCommandHandler(IRegistrationService registrationService, IUserRepository userRepository, IUnitOfWork unitOfWork, IEnumerable<IValidator<CreateUserCommand>> validators, IPermissionCheck<IClientRequest> permissionCheck) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _registrationService = registrationService;
    }

    public override async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var pipelineResult = await base.Handle(request, cancellationToken);
        if (pipelineResult.IsFailure)
            return new Result<CreateUserDto>(default, false, pipelineResult.Error);

        var user = new User()
        {
            Name = request.Name,
            Email = request.Email
        };

        try
        {

            var users = await _userRepository.GetUsersAsync(cancellationToken);
            if (users.Any(x => x.Email == request.Email))
               return new Result<CreateUserDto>(null, false);

            var guid = await _userRepository.InsertUserAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            try
            {
                await _registrationService.RegisterAsync(new RegisterUserDto(user.Name, user.Email));
            }
            catch
            {

            }

            return new Result<CreateUserDto>(new CreateUserDto(guid), true);
        }
        catch
        {
            return new Result<CreateUserDto>(null, false, "An error occured while creationg user.");
        }
    }
}