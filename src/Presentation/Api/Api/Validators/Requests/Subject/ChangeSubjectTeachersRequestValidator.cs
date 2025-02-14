using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Subject;

public class ChangeSubjectTeachersRequestValidator : AbstractValidator<ChangeSubjectTeachersRequest>
{
	public ChangeSubjectTeachersRequestValidator()
	{
		RuleForEach(x => x.NewSubjectTeachers).SetValidator(new UpdateTeacherDtoValidator());
		RuleFor(x => x.NewSubjectTeachers)
			.Must(x => x.Count <= 10)
			.WithMessage("Number of {PropertyName} cannot be greater than 10");
	}
}