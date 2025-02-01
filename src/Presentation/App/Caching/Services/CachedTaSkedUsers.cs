using Refit;
using TaSked.Api.ApiClient;
using TaSked.Domain;
using Akavache;
using System.Reactive.Linq;
using Microsoft.Maui.Networking;

namespace TaSked.App.Caching;

public class CachedTaSkedUsers : ITaSkedUsers
{
	private readonly IBlobCache Cache;
    private readonly ITaSkedSevice _api;
	private readonly IConnectivity _connectivity;
	private readonly IUserTokenStore _tokenStore;

	private const string CURENT_USER_CACHE_KEY = "CurrentUser";

	public CachedTaSkedUsers(IBlobCache cache, ITaSkedSevice api, IUserTokenStore tokenStore, IConnectivity connectivity)
	{
		Cache = cache;
		_api = api;
		_tokenStore = tokenStore;
		_connectivity = connectivity;

		if (_connectivity.NetworkAccess == NetworkAccess.Internet && tokenStore.AccessToken != null)
		{
            User user = api.CurrentUser().Result;
            Cache.InsertObject<User>(CURENT_USER_CACHE_KEY, user);
		}
	}

    public async Task<User> CurrentUser() 
    {
	    var user = await Cache.GetOrFetchObject(CURENT_USER_CACHE_KEY, () => _api.CurrentUser());
        return user;
    }

	public async Task<User> GetUserById(Guid id) {
        return await _api.GetUserById(id);
    }
}
