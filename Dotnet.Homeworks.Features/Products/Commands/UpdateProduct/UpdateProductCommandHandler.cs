using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.Domain.Exceptions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Shared.Dto;
using MediatR;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _productRepository = unitOfWork.ProductRepository;
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
            await _productRepository.UpdateProductAsync(updatedEntity, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

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