﻿using FluentValidation;
using TaSked.Api.Requests;

namespace Api.Validators.ValueObjects.Requests.Homework;

public class CreateHomeworkRequestValidator : AbstractValidator<CreateHomeworkRequest>
{
	public CreateHomeworkRequestValidator()
	{
		RuleFor(x => x.Title).NotEmpty().Length(2, 50);
		RuleFor(x => x.Description).NotNull().MaximumLength(4096);
		RuleForEach(x => x.RelatedLinks)
			.SetValidator(new RelatedLinkValidator())
			.When(x => x.RelatedLinks is not null);
		RuleFor(x => x.RelatedLinks)
			.Must(x => x!.Count <= 10)
			.When(x => x.RelatedLinks is not null)
			.WithMessage("Number of {PropertyName} cannot be greater than 10");
		RuleFor(x => x.BriefSummary)
			.MaximumLength(150)
			.When(x => x.BriefSummary is not null);
	}
}