using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;

namespace TaSked.Infrastructure.PushNotifications;

public static class DependencyInjection
{
	public static IServiceCollection AddFirebaseNotifications(this IServiceCollection services,
		string firebaseCredentials)
	{
		if (string.IsNullOrEmpty(firebaseCredentials))
		{
			throw new ArgumentNullException(nameof(firebaseCredentials));
		}

		var credentials = GoogleCredential.FromFile(firebaseCredentials);

		if (FirebaseApp.DefaultInstance == null)
		{
			FirebaseApp.Create(new AppOptions { Credential = credentials });
		}
		return services;
	}
}
