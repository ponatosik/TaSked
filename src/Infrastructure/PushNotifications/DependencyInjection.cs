using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;

namespace TaSked.Infrastructure.PushNotifications;

public static class DependencyInjection
{
	public static IServiceCollection AddFirebaseNotifications(this IServiceCollection services, GoogleCredential credentials)
	{
		FirebaseApp.Create(new AppOptions()
		{
			Credential = credentials
		});
		return services;
	}
}
