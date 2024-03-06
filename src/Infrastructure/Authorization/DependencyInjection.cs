using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using TaSked.Application;

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
				//ValidateAudience = false,
				//ValidateIssuer = false,
				//ValidateLifetime = false,
				//ValidateIssuerSigningKey = false,
				//ValidateActor = false,
				//ValidateSignatureLast = false,
				//ValidateTokenReplay = false,
				//ValidateWithLKG = false,
				//IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opt.SecretKey))


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
}
