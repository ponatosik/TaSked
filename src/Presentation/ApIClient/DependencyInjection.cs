using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace TaSked.Api.ApiClient;

public static class DependencyInjection
{
	public static IServiceCollection AddHomeworkApiClient(this IServiceCollection services, Action<HomeworkApiOptions> clientOptions)
	{
		var options = new HomeworkApiOptions();
		clientOptions(options);

		services.AddRefitClient<ITaskedApiClient>().
			ConfigureHttpClient(client => {
				client.BaseAddress = new Uri(options.BaseUrl);
				client.Timeout = options.Timeout;
			});

		return services;
	}
}
