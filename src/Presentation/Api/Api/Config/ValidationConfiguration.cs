using Api.Validators;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace TaSked.Api.Configuration;

public static class ValidationConfiguration
{
	public static IServiceCollection AddControllerInputValidation(this IServiceCollection services)
	{
		ValidatorOptions.Global.LanguageManager.Enabled = false;

		services.AddValidatorsFromAssemblyContaining<CreateAnonymousUserTokenRequestValidator>();
		services.AddFluentValidationAutoValidation();
		return services;
	}
}