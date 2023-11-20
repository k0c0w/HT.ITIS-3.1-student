using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;
internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{

    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            // name must not contain this symbols
            .Matches(@"[^$&+,:;=?@#|<>.-^*)(%!\""/№_}\[\]{{~]*")
            .MaximumLength(256);
    }
}
