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
	public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, Action<JwtOptions> options)
	{
		var opt = new JwtOptions();
		options(opt);

		services.AddSingleton(opt);
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
				ValidIssuer = opt.Issuer,
				ValidateAudience = true,
				ValidAudience = opt.Audience,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.SecretKey))
			};
			o.RequireHttpsMetadata = false;
		});


		return services;
	}


	public static IServiceCollection AddPolicyBasedAuthorization(this IServiceCollection services)
	{
		services.AddScoped<IAuthorizationHandler, MinimalGroupRoleRequirmentHandler>();
		services.AddAuthorization(options =>
		{
			options.AddPolicy(AccessPolicise.Member, policy => policy.Requirements.Add(new MinimalGroupRoleRequirment(GroupRole.Member)));
			options.AddPolicy(AccessPolicise.Moderator, policy => policy.Requirements.Add(new MinimalGroupRoleRequirment(GroupRole.Moderator)));
			options.AddPolicy(AccessPolicise.Admin, policy => policy.Requirements.Add(new MinimalGroupRoleRequirment(GroupRole.Admin)));
		});

		return services;
	}
}
