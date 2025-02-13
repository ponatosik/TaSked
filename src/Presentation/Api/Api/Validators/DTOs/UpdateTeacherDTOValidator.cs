using FluentValidation;
using TaSked.Domain;

namespace Api.Validators;

public class UpdateTeacherDtoValidator : AbstractValidator<UpdateTeacherDTO>
{
	public UpdateTeacherDtoValidator()
	{
		RuleFor(x => x.FullName)
			.NotEmpty()
			.Length(1, 50);

		RuleFor(x => x.Description)
			.MaximumLength(200)
			.When(x => x.Description is not null);

		RuleFor(x => x.PhoneNumber)
			.MaximumLength(17)
			.Matches(@"^\+?[\d- \(\)]*\d+$$")
			.When(x => x.PhoneNumber is not null);

		RuleFor(x => x.Email)
			.EmailAddress()
			.When(x => x.Email is not null);
	}
}