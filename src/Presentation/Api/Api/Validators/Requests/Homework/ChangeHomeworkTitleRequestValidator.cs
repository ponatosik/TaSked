using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Homework;

public class ChangeHomeworkTitleRequestValidator : AbstractValidator<ChangeHomeworkTitleRequest>
{
	public ChangeHomeworkTitleRequestValidator()
	{
		RuleFor(x => x.HomeworkTitle).NotEmpty().Length(2, 50);
	}
}