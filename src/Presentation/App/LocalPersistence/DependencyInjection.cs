using Microsoft.Extensions.DependencyInjection;

namespace TaSked.Infrastructure.LocalPersistence;

public static class DependencyInjection
{
	public static IServiceCollection AddLocalPersistence(this IServiceCollection services, string databaseFolder)
	{
		services.AddSingleton<TasksDatabase>(x => new TasksDatabase(databaseFolder));
		return services;
	}
}
