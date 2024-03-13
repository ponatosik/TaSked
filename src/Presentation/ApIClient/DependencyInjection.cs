using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace TaSked.Api.ApiClient;

public static class DependencyInjection
{
	public static IServiceCollection AddTaSkedApi(this IServiceCollection services, Action<HomeworkApiOptions> clientOptions)
	{
		var options = new HomeworkApiOptions();
		clientOptions(options);

		services.AddSingleton<HttpMessageHandler>();


		var serializer = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
		serializer.TypeInfoResolver = new DefaultJsonTypeInfoResolver()
		{
			Modifiers = { PrivateConstructorDeserializationModifier.UsePrivateConstructor }
		};

		RefitSettings settings = new RefitSettings()
		{
			ContentSerializer = new SystemTextJsonContentSerializer(serializer)
		};

		services.AddRefitClient<ITaSkedSevice>(settings).
			AddHttpMessageHandler<HttpMessageHandler>().
			ConfigureHttpClient(client =>
			{
				client.BaseAddress = new Uri(options.BaseUrl);
				client.Timeout = options.Timeout;
			});

		return services;
	}
}
