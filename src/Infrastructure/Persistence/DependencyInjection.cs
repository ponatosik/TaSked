using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaSked.Application.Data;
using TaSked.Infrastructure.Persistence.AzureMySqlInApp;

namespace TaSked.Infrastructure.Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistence(
		this IServiceCollection services,
		Action<DbContextOptionsBuilder>? configureOptions)
	{
		services.AddDbContext<ApplicationDbContext>(configureOptions);
		services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
		return services;
	}

	public static IServiceCollection AddPersistence(
		this IServiceCollection services,
		IConfigurationManager configuration)
	{
		var useAzureMySqlInApp = configuration["UseAzureMySqlInApp"]?.ToLower() == "true";

		if (useAzureMySqlInApp)
		{
			return AddPersistence(services, opt => opt.UseAzureMysqlInApp());
		}

		return services.AddPersistence(_ => { });
	}
}