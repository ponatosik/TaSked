using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Groups;

public class CreateGroupRequestValidator : AbstractValidator<CreateGroupRequest>

{
	public CreateGroupRequestValidator()
	{
		RuleFor(x => x.GroupName).NotEmpty().Length(2, 50);
	}
}