using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Announcements;

public class CreateAnnouncementRequestValidator : AbstractValidator<CreateAnnouncementRequest>
{
	public CreateAnnouncementRequestValidator()
	{
		RuleFor(x => x.Title).NotEmpty().Length(4, 150);
		RuleFor(x => x.Message).NotEmpty().MaximumLength(4096);
	}
}