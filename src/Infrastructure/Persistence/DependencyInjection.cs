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
		var connectionString = configuration.GetConnectionString("TaSkedDb");
		if (connectionString is not null)
		{
			return services.AddPersistence(opt => opt.UseNpgsql(connectionString));
		}
		
		var useAzureMySqlInApp = configuration["UseAzureMySqlInApp"]?.ToLower() == "true";
		if (useAzureMySqlInApp)
		{
			return services.AddPersistence(opt => opt.UseAzureMysqlInApp());
		}

		var useInMemoryDatabase = configuration["UseInMemoryDb"]?.ToLower() == "true";
		if (useInMemoryDatabase)
		{
			return services.AddPersistence(opt => opt.UseInMemoryDatabase("TaSkedDb-in-memory"));
		}

		return services.AddPersistence(_ => { });
	}
}