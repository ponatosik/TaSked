using Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.Infrastructure.Authorization;

public static class DependencyInjection
{
	public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
	{
		services.AddSingleton(jwtOptions);
		services.AddSingleton<IJwtProvider, JwtProvider>();

		services.AddAuthentication(options =>
		{
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(o =>
		{
			o.TokenValidationParameters = new()
			{
				ValidateIssuer = true,
				ValidIssuer = jwtOptions.Issuer,
				ValidateAudience = true,
				ValidAudience = jwtOptions.Audience,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
			};
			o.RequireHttpsMetadata = false;
		});


		return services;
	}


	public static IServiceCollection AddPolicyBasedAuthorization(this IServiceCollection services)
	{
		services.AddScoped<IAuthorizationHandler, MinimalGroupRoleRequirementHandler>();
		services.AddAuthorizationBuilder()
			.AddPolicy(AccessPolicies.Member,
				policy => policy.Requirements.Add(new MinimalGroupRoleRequirment(GroupRole.Member)))
			.AddPolicy(AccessPolicies.Moderator,
				policy => policy.Requirements.Add(new MinimalGroupRoleRequirment(GroupRole.Moderator)))
			.AddPolicy(AccessPolicies.Admin,
				policy => policy.Requirements.Add(new MinimalGroupRoleRequirment(GroupRole.Admin)));

		return services;
	}
}
