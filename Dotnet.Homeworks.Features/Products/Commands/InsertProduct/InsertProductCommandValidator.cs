using FluentValidation;

namespace Dotnet.Homeworks.Features.Products.Commands.InsertProduct;
internal class InsertProductCommandValidator : AbstractValidator<InsertProductCommand>
{

    public InsertProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty();
    }
}
