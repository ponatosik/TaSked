using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Subject;

public class ChangeSubjectLinksRequestValidator : AbstractValidator<ChangeSubjectLinksRequest>
{
	public ChangeSubjectLinksRequestValidator()
	{
		RuleForEach(x => x.NewLinks).SetValidator(new RelatedLinkValidator());
		RuleFor(x => x.NewLinks)
			.Must(x => x.Count <= 10)
			.WithMessage("Number of {PropertyName} cannot be greater than 10");
	}
}