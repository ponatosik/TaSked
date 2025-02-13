using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Lesson;

public class ChangeLessonLinkRequestValidator : AbstractValidator<ChangeLessonLinkRequest>
{
	public ChangeLessonLinkRequestValidator()
	{
		RuleFor(x => x.NewLink)
			.SetValidator(new RelatedLinkValidator()!)
			.When(x => x.NewLink is not null);
	}
}