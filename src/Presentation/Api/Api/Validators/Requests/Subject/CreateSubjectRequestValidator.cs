using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Subject;

public class CreateSubjectRequestValidator : AbstractValidator<CreateSubjectRequest>
{
	public CreateSubjectRequestValidator()
	{
		RuleFor(x => x.SubjectName).NotEmpty().Length(2, 50);
		RuleForEach(x => x.RelatedLinks).SetValidator(new RelatedLinkValidator());
		RuleFor(x => x.RelatedLinks)
			.Must(x => x!.Count <= 10)
			.When(x => x.RelatedLinks is not null)
			.WithMessage("Number of {PropertyName} cannot be greater than 10");
	}
}