using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Splat;
using TaSked.Api.ApiClient;
using TaSked.Application;
using TaSked.Domain;

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
		services.AddSingleton<ITaSkedLessons, CachedTaSkedLessons>();
		services.AddSingleton<ITaSkedReports, CachedTaSkedReports>();

		services.AddSingleton<CachedRepository<SubjectDTO>, CachedTaSkedSubjects>();
		services.AddSingleton<CachedRepository<Homework>, CachedTaSkedHomeworks>();
		services.AddSingleton<CachedRepository<Invitation>, CachedTaSkedInvitations>();
		services.AddSingleton<CachedRepository<Lesson>, CachedTaSkedLessons>();
		services.AddSingleton<CachedRepository<Report>, CachedTaSkedReports>();

		return services;
	}
}
