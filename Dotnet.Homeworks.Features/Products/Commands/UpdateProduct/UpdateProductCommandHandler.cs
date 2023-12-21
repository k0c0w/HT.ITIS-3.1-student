using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Exceptions;
using Dotnet.Homeworks.Features.Mapster.MapServices.Abstractions;
using Dotnet.Homeworks.Infrastructure.Cqrs.Commands;
using Dotnet.Homeworks.Infrastructure.UnitOfWork;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;
    private readonly IProductMapper _productMapper;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IProductRepository productRepository, IProductMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _productMapper = mapper;
    }

    async Task<Result> IRequestHandler<UpdateProductCommand, Result>.Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(request.Guid, cancellationToken);
            if (product == null)
                return new Result(false, $"Product ({request.Guid}) does not exist.");

            product = _productMapper.MapFromUpdateCommandToExisting(request, product);

            await _productRepository.UpdateProductAsync(product, cancellationToken);
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
    }
}