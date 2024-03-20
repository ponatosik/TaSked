using Microsoft.Extensions.DependencyInjection;

namespace TaSked.Infrastructure.LocalPersistence;

public static class DependencyInjection
{
	public static IServiceCollection AddLocalPersistence(this IServiceCollection services, LocalPersistenceOptions options)
	{
		services.AddSingleton<TasksDatabase>();
		services.AddSingleton<LocalPersistenceOptions>();
		return services;
	}
}
