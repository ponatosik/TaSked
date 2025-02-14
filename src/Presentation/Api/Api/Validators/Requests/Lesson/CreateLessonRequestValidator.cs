using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Lesson;

public class CreateLessonRequestValidator : AbstractValidator<CreateLessonRequest>
{
	public CreateLessonRequestValidator()
	{
		RuleFor(x => x.LessonLink)
			.SetValidator(new RelatedLinkValidator()!)
			.When(x => x.LessonLink is not null);
	}
}