using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Homework;

public class CommentHomeworkRequestValidator : AbstractValidator<CommentHomeworkRequest>
{
	public CommentHomeworkRequestValidator()
	{
		RuleFor(x => x.Content).NotEmpty().Length(2, 512);
	}
}