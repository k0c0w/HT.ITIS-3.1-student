﻿using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Features.Decorators;
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

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IEnumerable<IValidator<CreateUserCommand>> validators, IPermissionCheck<IClientRequest> permissionCheck) : base(validators, permissionCheck)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<CreateUserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var pipelineResult = await base.Handle(request, cancellationToken);
        if (pipelineResult.IsFailure)
            return pipelineResult;

        if (cancellationToken.IsCancellationRequested)
            return new Result<CreateUserDto>(null, false);

        var user = new User()
        {
            Name = request.Name,
            Email = request.Email
        };

        try
        {
            var guid = await _userRepository.InsertUserAsync(user, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return new Result<CreateUserDto>(null, false);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                return new Result<CreateUserDto>(null, false);


            return new Result<CreateUserDto>(new CreateUserDto(guid), false);
        }
        catch
        {
            return new Result<CreateUserDto>(null, false, "An error occured while creationg user.");
        }
    }
}