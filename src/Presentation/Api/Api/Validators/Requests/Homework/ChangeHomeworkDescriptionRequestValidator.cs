using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Homework;

public class ChangeHomeworkDescriptionRequestValidator : AbstractValidator<ChangeHomeworkDescriptionRequest>
{
	public ChangeHomeworkDescriptionRequestValidator()
	{
		RuleFor(x => x.HomeworkDescription).NotEmpty().MaximumLength(4096);
	}
}