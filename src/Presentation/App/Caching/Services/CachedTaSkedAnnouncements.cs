using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Caching;

public class CachedTaSkedAnnouncements : CachedRepository<Announcement>, ITaSkedAnnouncements
{
	private readonly ITaSkedService _api;

	public CachedTaSkedAnnouncements(IBlobCache cache, ITaSkedService api, IConnectivity connectivity) : base(cache)
    {
	    _api = api;

	    if(connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_ = FetchAndCacheEntities();
		}
    }

	public async Task<Announcement> CreateAnnouncement(CreateAnnouncementRequest request)
    {
	    var announcement = await _api.CreateAnnouncement(request);
	    await CacheEntityAsync(announcement);
	    return announcement;
    }

	public async Task<List<Announcement>> GetAllAnnouncements()
    {
		var allLessons = (await GetCachedEntities()).ToList();
		if(allLessons.Count == 0) 
		{
			allLessons = (await FetchAndCacheEntities()).ToList();
		}
		return allLessons;
    }

	protected override async Task<IEnumerable<Announcement>> FetchEntities()
	{
		return await _api.GetAllAnnouncements();
	}

	protected override string GetEntityKey(Announcement entity)
	{
		return entity.Id.ToString();
	}
}
