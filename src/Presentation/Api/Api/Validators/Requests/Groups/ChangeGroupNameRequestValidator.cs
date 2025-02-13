using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Groups;

public class ChangeGroupNameRequestValidator : AbstractValidator<ChangeGroupNameRequest>
{
	public ChangeGroupNameRequestValidator()
	{
		RuleFor(x => x.GroupName).NotEmpty().Length(2, 50);
	}
}