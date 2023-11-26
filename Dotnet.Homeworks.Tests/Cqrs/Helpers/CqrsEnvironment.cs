using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.MainProject.Controllers;
using MassTransit;

namespace Dotnet.Homeworks.Tests.Cqrs.Helpers;

internal class CqrsEnvironment
{
    public CqrsEnvironment(ProductManagementController productManagementController, IUnitOfWork unitOfWorkMock,
        MediatR.IMediator mediatR, Mediator.IMediator customMediator,
        IUserRepository userRepository, IBus bus = default)
    {
        ProductManagementController = productManagementController;
        CustomMediator = customMediator;
        UserRepository = userRepository;
        MediatR = mediatR;
        UnitOfWorkMock = unitOfWorkMock;
        Bus = bus;
    }

    public ProductManagementController ProductManagementController { get; }
    public IUnitOfWork UnitOfWorkMock { get; }
    public MediatR.IMediator MediatR { get; }
    public Mediator.IMediator CustomMediator { get; }
    public IUserRepository UserRepository { get; }

    public IBus? Bus { get; } 
}