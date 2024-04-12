using Refit;
using TaSked.Api.ApiClient;
using TaSked.Domain;
using Akavache;
using System.Reactive.Linq;

namespace TaSked.App.Common.Caching;

public class CachedTaSkedUsers : ITaSkedUsers
{
	private IBlobCache Cache = ServiceHelper.GetService<IBlobCache>();

    private readonly ITaSkedSevice _api;
	private readonly IConnectivity _connectivity;

	private const string CURENT_USER_CACHE_KEY = "CurrentUser";

	public CachedTaSkedUsers(ITaSkedSevice api, IConnectivity connectivity)
	{
		_api = api;
		_connectivity = connectivity;

		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
            User user = api.CurrentUser().Result;
            Cache.InsertObject<User>(CURENT_USER_CACHE_KEY, user);
		}
	}

    public async Task<User> CurrentUser() 
    {
		var user = await Cache.GetObject<User>(CURENT_USER_CACHE_KEY);
        return user;
    }

	public async Task<User> GetUserById(Guid id) {
        return await _api.GetUserById(id);
    }
}
