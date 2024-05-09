using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using TaSked.App.Common;
using TaSked.Domain;

namespace TaSked.App;

public partial class TaskViewModel : ReactiveObject
{
	private HomeworkTask _task;
	public HomeworkTask Task
	{
		get => _task;
		set => this.RaiseAndSetIfChanged(ref _task, value);
	}

	private string _subjectName;
	public string SubjectName
	{
		get => _subjectName;
		set => this.RaiseAndSetIfChanged(ref _subjectName, value);
	}

	public TaskViewModel() { }
	public TaskViewModel(HomeworkTask task, string subjectName)
	{
		Task = task;
		SubjectName = subjectName;
	}

	[RelayCommand]
	private async Task Complete()
	{
		HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
		await tasksService.CompleteAsync(Task);
		ServiceHelper.Services.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(this);
	}

	[RelayCommand]
	private async Task UndoCompletion()
	{
		HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
		await tasksService.UndoCompletionAsync(Task);
		ServiceHelper.Services.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(this);
	}

    [RelayCommand]
    private async Task UpdateHomework()
    {
        await Shell.Current.GoToAsync("UpdateTaskPage", new Dictionary<string, object>
		{
			["homework"] = Task.Homework
		});
    }
}
