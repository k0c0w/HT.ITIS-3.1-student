using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.CreateUser;
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{

    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress(FluentValidation.Validators.EmailValidationMode.Net4xRegex);

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            // name must not contain this symbols
            .Matches(@"[^$&+,:;=?@#|<>.-^*)(%!\""/№_}\[\]{{~]*")
            .MaximumLength(256);
    }
}
