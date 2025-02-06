using Akavache;
using Microsoft.Maui.Networking;
using System.Reactive.Linq;
using TaSked.Api.ApiClient;
using TaSked.Domain;

namespace TaSked.App.Caching;

public class CachedTaSkedUsers : ITaSkedUsers
{
	private readonly IBlobCache _cache;
	private readonly ITaSkedService _api;
	private readonly IUserTokenStore _tokenStore;

	private const string CURENT_USER_CACHE_KEY = "CurrentUser";

	public CachedTaSkedUsers(IBlobCache cache, ITaSkedService api, IUserTokenStore tokenStore,
		IConnectivity connectivity)
	{
		_cache = cache;
		_api = api;
		_tokenStore = tokenStore;

		if (connectivity.NetworkAccess == NetworkAccess.Internet && tokenStore.AccessToken != null)
		{
			var user = api.GetCurrentUser().Result;
            _cache.InsertObject<User>(CURENT_USER_CACHE_KEY, user);
		}
	}

	public async Task<User> GetCurrentUser() 
    {
		var user = await _cache.GetObject<User>(CURENT_USER_CACHE_KEY);
        return user;
    }

	public async Task<User> GetUserById(Guid id) {
        return await _api.GetUserById(id);
    }
}
