using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using System.Reactive.Linq;
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

		CompleteCommand = ReactiveCommand.CreateFromTask(Complete, Observable.Return(false));
		UndoCompletionCommand = ReactiveCommand.CreateFromTask(UndoCompletion, this.WhenAnyValue(x => x.Task, t => t.Completed));
		UpdateHomeworkCommand = ReactiveCommand.CreateFromTask(UpdateHomework);
	}

	private IReactiveCommand _completedCommand;
	public IReactiveCommand CompleteCommand 
	{
		get => _completedCommand;
		set => this.RaiseAndSetIfChanged(ref _completedCommand, value);
	}

	private IReactiveCommand _undoCompletionCommand;
	public IReactiveCommand UndoCompletionCommand
	{
		get => _undoCompletionCommand;
		set => this.RaiseAndSetIfChanged(ref _undoCompletionCommand, value);
	}

	private IReactiveCommand _updateHomeworkCommand;
	public IReactiveCommand UpdateHomeworkCommand 
	{
		get => _updateHomeworkCommand;
		set => this.RaiseAndSetIfChanged(ref _updateHomeworkCommand, value);
	}

	private async Task Complete()
	{
		HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
		await tasksService.CompleteAsync(Task);
		ServiceHelper.Services.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(this);
	}

	private async Task UndoCompletion()
	{
		HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
		await tasksService.UndoCompletionAsync(Task);
		ServiceHelper.Services.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(this);
	}

    private async Task UpdateHomework()
    {
        await Shell.Current.GoToAsync("UpdateTaskPage", new Dictionary<string, object>
		{
			["homework"] = Task.Homework
		});
    }
}
