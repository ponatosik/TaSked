using DynamicData;
using TaSked.Api.ApiClient;
using TaSked.Application;

namespace TaSked.App;

public class SubjectDataSource
{
	private readonly ITaSkedSubjects _api;

	public SourceCache<SubjectViewModel, Guid> SubjectSource { get; set; } = 
		new SourceCache<SubjectViewModel, Guid>(model => model.SubjectDTO.Id);

	public SubjectDataSource(ITaSkedSubjects api)
	{
		_api = api;
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
}
