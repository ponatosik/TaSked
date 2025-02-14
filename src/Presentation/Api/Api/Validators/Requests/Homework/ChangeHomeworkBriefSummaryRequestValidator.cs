using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Homework;

public class ChangeHomeworkBriefSummaryRequestValidator : AbstractValidator<ChangeHomeworkBriefSummaryRequest>
{
	public ChangeHomeworkBriefSummaryRequestValidator()
	{
		RuleFor(x => x.BriefSummary)
			.MaximumLength(150)
			.When(x => x.BriefSummary is not null);
	}
}