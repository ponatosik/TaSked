using DynamicData;
using TaSked.App.Caching;
using TaSked.App.Common;
using TaSked.Domain;

namespace TaSked.App;

public class HomeworkDataSource
{
	private readonly HomeworkTasksService _homeworkService;
	private readonly SubjectDataSource _subjectDataSource;
    private readonly IConnectivity _connectivity;
	private readonly CachedRepository<Homework>? _homeworkCache;

	public SourceCache<TaskViewModel, Guid> HomeworkSource { get; set; } =
		new SourceCache<TaskViewModel, Guid>(viewModel => viewModel.Task.Homework.Id);

	public HomeworkDataSource(
		HomeworkTasksService homeworkService, 
		SubjectDataSource subjectDataSource,
		IConnectivity connectivity,
		CachedRepository<Homework>? homeworkCache = null)
	{
		_homeworkService = homeworkService;
		_subjectDataSource = subjectDataSource;
		_connectivity = connectivity;
		_homeworkCache = homeworkCache;
		Task.Run(UpdateAsync);
	}

	public async Task UpdateAsync()
	{
		var tasks = await _homeworkService.GetAllAsync(); 

		HomeworkSource.Edit(source =>
		{
			source.Clear();
			tasks.ForEach(task =>
				source.AddOrUpdate(
					new TaskViewModel(task, 
					// TODO: Don't use Items. Data stream instead
					_subjectDataSource.SubjectSource.Items
					.FirstOrDefault(s => s.SubjectDTO.Id == task.Homework.SubjectId)?.SubjectDTO.Name ?? "Unknown")));
		});
	}

	public async Task ForceUpdateAsync()
	{
		if(_connectivity.NetworkAccess == NetworkAccess.Internet)
		{
			_homeworkCache?.ClearCache();
		}
		await UpdateAsync();	
	}
}
