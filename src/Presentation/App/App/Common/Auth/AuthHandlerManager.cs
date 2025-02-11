namespace TaSked.App.Common;

public class AuthHandlerManager : IAuthHandlerManager
{
	private const string AuthenticationMethodKey = "TaSked.AuthenticationMethod";

	private readonly ISecureStorage _storage;

	public AuthHandlerManager(ISecureStorage storage)
	{
		_storage = storage;
	}

	public async Task<IAuthHandler?> LoadCurrentAuthHandlerAsync()
	{
		var type = await _storage.GetAsync(AuthenticationMethodKey);
		if (type is null)
		{
			return null;
		}

		var services = ServiceHelper.Services;
		return type switch
		{
			AppAuthMethods.Anonymous => services.GetService<AnonymousAuthorizationHandler>(),
			AppAuthMethods.Auth0 => services.GetService<Auth0AuthHandler>(),
			_ => throw new InvalidCastException("Unknown authentication method")
		};
	}

	public Task SaveCurrentAuthHandlerAsync(IAuthHandler authHandler)
	{
		var type = authHandler switch
		{
			AnonymousAuthorizationHandler => AppAuthMethods.Anonymous,
			Auth0AuthHandler => AppAuthMethods.Auth0,
			_ => throw new InvalidCastException("Unknown authentication method")
		};
		return _storage.SetAsync(AuthenticationMethodKey, type);
	}

	public Task RemoveCurrentAuthHandlerAsync()
	{
		_storage.Remove(AuthenticationMethodKey);
		return Task.CompletedTask;
	}
}