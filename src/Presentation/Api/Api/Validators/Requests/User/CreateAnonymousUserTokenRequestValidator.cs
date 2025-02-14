using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators;

public sealed class CreateAnonymousUserTokenRequestValidator : AbstractValidator<CreateAnonymousUserTokenRequest>
{
	public CreateAnonymousUserTokenRequestValidator()
	{
		RuleFor(x => x.Nickname).NotEmpty().Length(2, 24);
	}
}