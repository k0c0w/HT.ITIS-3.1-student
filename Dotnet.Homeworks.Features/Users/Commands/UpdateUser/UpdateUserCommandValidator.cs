using FluentValidation;

namespace Dotnet.Homeworks.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(x => x.User)
			.NotNull();
		RuleFor(x => x.User.Name)
			.NotNull()
			.NotEmpty()
			// name must not contain this symbols
			.Matches(@"[^$&+,:;=?@#|<>.-^*)(%!\""/№_}\[\]{{~]*")
			.MaximumLength(256);

		RuleFor(x => x.User.Email)
			.NotNull()
			.NotEmpty()
			.EmailAddress();

		RuleFor(x => x.Guid)
			.Must(x => x != default);
	}
}
