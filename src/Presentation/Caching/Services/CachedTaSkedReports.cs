using Akavache;
using Microsoft.Maui.Networking;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Caching;

public class CachedTaSkedReports : CachedRepository<Report>, ITaSkedReports
{
	private readonly ITaSkedSevice _api;
    private readonly IConnectivity _connectivity;

    public CachedTaSkedReports(IBlobCache cache, ITaSkedSevice api, IConnectivity connectivity) : base(cache)
    {
        _api = api; 
        _connectivity = connectivity;
        
		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			FetchAndCacheEntities();
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
		return (await GetCachedEntities()).ToList();
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
