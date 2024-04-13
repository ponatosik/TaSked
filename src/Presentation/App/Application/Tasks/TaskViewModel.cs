using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.App.Common;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

public partial class TaskViewModel : ObservableObject
{
	[ObservableProperty]
	private HomeworkTask _task;

	[ObservableProperty]
	private string _subjectName;

	public TaskViewModel() { }
	public TaskViewModel(HomeworkTask task, string subjectName)
	{
		_task = task;
		_subjectName = subjectName;
	}

	[RelayCommand]
	private async Task Complete()
	{
		HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
		await tasksService.CompleteAsync(Task);
	}

	[RelayCommand]
	private async Task UndoCompletion()
	{
		HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
		await tasksService.UndoCompletionAsync(Task);
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
