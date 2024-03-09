using Microsoft.OpenApi.Models;

namespace TaSked.Api.Configuration;

public static class SwaggerConfiguration
{
	public static void AddSwaggerConfiguration(this IServiceCollection services)
	{
		services.AddSwaggerGen(x =>
		{
			x.SwaggerDoc("Documentation", new OpenApiInfo { Title = "TaSked", Description = "Documentation for TaSked" });

			x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				Name = "Authorization",
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "JWT Authorization header using the Bearer scheme. DONT FORGET TO PUT \"bearer\" BEFORE YOUR TOKEN."

			});
			x.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						  new OpenApiSecurityScheme
						  {
							  Reference = new OpenApiReference
							  {
								  Type = ReferenceType.SecurityScheme,
								  Id = "Bearer"
							  }
						  },
						 new string[] {}
					}
				});
		});
	}

	public static void UseSwaggerConfiguration(this IApplicationBuilder app)
	{
		app.UseSwagger();
		app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/Documentation/swagger.json", "TaSked testing API"));
	}
}
