using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Invitation;

public class CreateInvitationRequestValidator : AbstractValidator<CreateInvitationRequest>
{
	public CreateInvitationRequestValidator()
	{
		RuleFor(x => x.InvitationCaption)
			.NotEmpty()
			.Length(2, 50)
			.When(x => x.InvitationCaption is not null);

		RuleFor(x => x.MaxActivations)
			.GreaterThan(0)
			.When(x => x.MaxActivations is not null);
	}
}