using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Subject;

public class ChangeSubjectNameRequestValidator : AbstractValidator<ChangeSubjectNameRequest>
{
	public ChangeSubjectNameRequestValidator()
	{
		RuleFor(x => x.NewSubjectName).NotEmpty().Length(2, 50);
	}
}