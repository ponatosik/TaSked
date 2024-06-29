using DynamicData;
using TaSked.Api.ApiClient;
using TaSked.App.Caching;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

public class ReportDataSource
{
	private readonly ITaSkedReports _api;
    private readonly IConnectivity _connectivity;
	private readonly CachedRepository<Report>? _reportCache;

	public SourceCache<Report, Guid> ReportSource { get; set; } = 
		new SourceCache<Report, Guid>(model => model.Id);

	public ReportDataSource(ITaSkedReports api, IConnectivity connectivity, CachedRepository<Report>? reportCache = null)
	{
		_api = api;
		_connectivity = connectivity;
		_reportCache = reportCache;
		Task.Run(UpdateAsync);
	}

	public async Task UpdateAsync()
	{
		var reports = await _api.GetAllReports();

		ReportSource.Edit(source =>
		{
			source.Clear();
			reports.ForEach(report => source.AddOrUpdate(report));
		});
	}

	public async Task ForceUpdateAsync()
	{
		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_reportCache?.ClearCache();
		}
		await UpdateAsync();	
	}
}
