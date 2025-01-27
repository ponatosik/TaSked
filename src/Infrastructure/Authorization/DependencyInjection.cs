using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.Infrastructure.Authorization;

public static class DependencyInjection
{
	
	public static IHostApplicationBuilder AddJwtAuthentication(this IHostApplicationBuilder builder)
	{
		var jwtOptionsSection = builder.Configuration.GetRequiredSection(JwtOptions.SectionName);
		var jwtOptions = jwtOptionsSection.Get<JwtOptions>() ??
		                 throw new ValidationException("JwtOptions not found");

		builder.Services
			.AddOptions<JwtOptions>()
			.BindConfiguration(JwtOptions.SectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();

		builder.Services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
		builder.Services.AddSingleton<IJwtProvider, JwtProvider>();
		builder.Services.AddAuthentication().AddJwtBearer(AuthenticationSchemas.AnonymousSchema, o =>
		{
			o.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuers = jwtOptions.ValidIssuers.Split(',').Select(s => s.Trim()),
				ValidateAudience = true,
				ValidAudiences = jwtOptions.ValidAudiences.Split(',').Select(s => s.Trim()),
				ValidateIssuerSigningKey = true,
				ValidateLifetime = jwtOptions.ValidateLifetime,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
			};
			o.RequireHttpsMetadata = false;
		})
		.AddJwtBearer(AuthenticationSchemas.Auth0Schema);

		return builder;
	}


	public static IServiceCollection AddPolicyBasedAuthorization(this IServiceCollection services)
	{
		services.AddScoped<IAuthorizationHandler, MinimalGroupRoleRequirementHandler>();

		var multiAuthorizationPolicy =
			new AuthorizationPolicyBuilder(AuthenticationSchemas.AnonymousSchema, AuthenticationSchemas.Auth0Schema)
			.RequireAuthenticatedUser()
			.Build();

		services.AddAuthorizationBuilder()
			.SetDefaultPolicy(multiAuthorizationPolicy)
			.AddPolicy(AccessPolicies.Member,
				policy => policy.UseMultipleSchemas().Requirements
					.Add(new MinimalGroupRoleRequirment(GroupRole.Member)))
			.AddPolicy(AccessPolicies.Moderator,
				policy => policy.UseMultipleSchemas().Requirements
					.Add(new MinimalGroupRoleRequirment(GroupRole.Moderator)))
			.AddPolicy(AccessPolicies.Admin,
				policy => policy.UseMultipleSchemas().Requirements
					.Add(new MinimalGroupRoleRequirment(GroupRole.Admin)));

		return services;
	}

	public static AuthorizationPolicyBuilder UseMultipleSchemas(this AuthorizationPolicyBuilder policy)
	{
		policy.RequireClaim(CustomClaimTypes.UserId);
		policy.AddAuthenticationSchemes(AuthenticationSchemas.Auth0Schema, AuthenticationSchemas.AnonymousSchema);
		policy.RequireAuthenticatedUser();
		return policy;
	}
}
