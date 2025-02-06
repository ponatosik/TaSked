using DynamicData;
using TaSked.Api.ApiClient;
using TaSked.App.Caching;
using TaSked.Domain;

namespace TaSked.App;

public class AnnouncementDataSource
{
	private readonly ITaSkedAnnouncements _api;
    private readonly IConnectivity _connectivity;
    private readonly CachedRepository<Announcement>? _announcementCache;

    public SourceCache<Announcement, Guid> AnnouncementSource { get; set; } = new(model => model.Id);

    public AnnouncementDataSource(ITaSkedAnnouncements api, IConnectivity connectivity,
	    CachedRepository<Announcement>? announcementCache = null)
	{
		_api = api;
		_connectivity = connectivity;
		_announcementCache = announcementCache;
		Task.Run(UpdateAsync);
	}

	public async Task UpdateAsync()
	{
		var announcements = await _api.GetAllAnnouncements();

		AnnouncementSource.Edit(source =>
		{
			source.Clear();
			announcements.ForEach(announcement => source.AddOrUpdate(announcement));
		});
	}

	public async Task ForceUpdateAsync()
	{
		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_announcementCache?.ClearCache();
		}
		await UpdateAsync();	
	}
}
