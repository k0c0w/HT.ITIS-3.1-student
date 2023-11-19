using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Domain.Exceptions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    async Task<Result> IRequestHandler<UpdateProductCommand, Result>.Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var updatedEntity = new Product()
        {
            Id = request.Guid,
            Name = request.Name
        };

        try
        {
            await _productRepository.UpdateProductAsync(updatedEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new Result(true);
        }
        catch(EntityNotFoundException ex)
        {
            return new Result(false, ex.Message);
        }
        catch
        {
            return new Result(false);
        }
        finally
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}