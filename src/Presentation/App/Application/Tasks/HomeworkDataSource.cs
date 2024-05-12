using DynamicData;
using TaSked.App.Common;

namespace TaSked.App;

public class HomeworkDataSource
{
	private readonly HomeworkTasksService _homeworkService;
	private readonly SubjectDataSource _subjectDataSource;

	public SourceCache<TaskViewModel, Guid> HomeworkSource { get; set; } =
		new SourceCache<TaskViewModel, Guid>(viewModel => viewModel.Task.Homework.Id);

	public HomeworkDataSource(HomeworkTasksService homeworkService, SubjectDataSource subjectDataSource)
	{
		_homeworkService = homeworkService;
		_subjectDataSource = subjectDataSource;
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
}
