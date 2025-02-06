using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Caching;

public class CachedTaSkedReports : CachedRepository<Report>, ITaSkedReports
{
	private readonly ITaSkedService _api;

	public CachedTaSkedReports(IBlobCache cache, ITaSkedService api, IConnectivity connectivity) : base(cache)
    {
	    _api = api;

	    if(connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_ = FetchAndCacheEntities();
		}
    }

    public async Task<Report> CreateReport(CreateReportRequest request)
    {
		Report report = await _api.CreateReport(request);
		await CacheEntityAsync(report);
		return report;
    }

	public async Task<List<Report>> GetAllReports()
    {
		var allLessons = (await GetCachedEntities()).ToList();
		if(allLessons.Count == 0) 
		{
			allLessons = (await FetchAndCacheEntities()).ToList();
		}
		return allLessons;
    }

	protected override async Task<IEnumerable<Report>> FetchEntities()
	{
		return await _api.GetAllReports();
	}

	protected override string GetEntityKey(Report entity)
	{
		return entity.Id.ToString();
	}
}
