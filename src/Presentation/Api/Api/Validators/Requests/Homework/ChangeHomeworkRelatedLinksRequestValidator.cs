using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Homework;

public class ChangeHomeworkRelatedLinksRequestValidator : AbstractValidator<ChangeHomeworkRelatedLinksRequest>
{
	public ChangeHomeworkRelatedLinksRequestValidator()
	{
		RuleFor(x => x.RelatedLinks)
			.Must(x => x.Count <= 10)
			.WithMessage("Number of {PropertyName} cannot be greater than 10");
		RuleForEach(x => x.RelatedLinks).SetValidator(new RelatedLinkValidator());
	}
}