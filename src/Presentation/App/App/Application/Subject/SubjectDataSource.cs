using DynamicData;
using TaSked.Api.ApiClient;
using TaSked.App.Caching;
using TaSked.Application;

namespace TaSked.App;

public class SubjectDataSource
{
	private readonly ITaSkedSubjects _api;
    private readonly IConnectivity _connectivity;
	private readonly CachedRepository<SubjectDTO>? _subjectCache;

	public SourceCache<SubjectViewModel, Guid> SubjectSource { get; set; } = 
		new SourceCache<SubjectViewModel, Guid>(model => model.SubjectDTO.Id);

	public SubjectDataSource(ITaSkedSubjects api, IConnectivity connectivity, CachedRepository<SubjectDTO>? subjectCache = null)
	{
		_api = api;
		_connectivity = connectivity;
		_subjectCache = subjectCache;
		Task.Run(UpdateAsync);
	}

	public async Task UpdateAsync()
	{
		var subjects = await _api.GetAllSubjects();

		SubjectSource.Edit(source =>
		{
			source.Clear();
			subjects.ForEach(subject =>
				source.AddOrUpdate(new SubjectViewModel(subject)));
		});
	}

	public async Task ForceUpdateAsync()
	{
		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_subjectCache?.ClearCache();
		}
		await UpdateAsync();	
	}
}
