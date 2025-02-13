using FluentValidation;
using TaSked.Domain;

namespace Api.Validators.ValueObjects;

public sealed class RelatedLinkValidator : AbstractValidator<RelatedLink>
{
	public RelatedLinkValidator()
	{
		RuleFor(x => x.Title)
			.Length(2, 18)
			.When(x => x.Title is not null);

		RuleFor(x => x.Url)
			.NotEmpty()
			.Must(x => Uri.TryCreate(x?.ToString() ?? string.Empty, UriKind.Absolute, out _))
			.WithMessage("{PropertyName} must be a valid URL.");
	}
}