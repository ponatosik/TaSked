using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Splat;
using TaSked.Api.ApiClient;

namespace TaSked.App.Caching; 

public static class DependencyInjection
{
	public static IServiceCollection AddTaSkedCache(this IServiceCollection services)
	{
		var jsonSerializerSettings = new JsonSerializerSettings
		{
			ContractResolver = new JsonPrivatePropertiesResolver(),
			ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
		};
		Locator.CurrentMutable.RegisterConstant(jsonSerializerSettings, typeof(JsonSerializerSettings));

		services.AddHostedService<CacheHostedService>();
		services.AddSingleton(CacheHostedService.GetCache());
		services.AddSingleton<ITaSkedSubjects, CachedTaSkedSubjects>();
		services.AddSingleton<ITaSkedUsers, CachedTaSkedUsers>();
		services.AddSingleton<ITaSkedHomeworks, CachedTaSkedHomeworks>();
		services.AddSingleton<ITaSkedInvitations, CachedTaSkedInvitations>();
		services.AddSingleton<ITaSkedReports, CachedTaSkedReports>();

		return services;
	}
}
