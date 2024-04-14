using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.App.Common;
using TaSked.Application;
using TaSked.Domain;
using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class UncompletedTasksViewModel : ObservableObject
{
	private readonly ITaSkedSubjects _subjectService;
	private readonly HomeworkTasksService _tasksService;

	[ObservableProperty]
	private ObservableCollection<TaskViewModel> _tasks;

	public UncompletedTasksViewModel(ITaSkedSubjects subjectService, HomeworkTasksService taskService)
	{
		_subjectService = subjectService;
		_tasksService = taskService;
		_tasks = new ObservableCollection<TaskViewModel>();
		LoadTasks();
	}

	private async Task LoadTasks()
	{
		List<HomeworkTask> tasks = new List<HomeworkTask>();
		List<SubjectDTO> subjects = new List<SubjectDTO>();

		tasks = await _tasksService.GetAllAsync();
		subjects = await _subjectService.GetAllSubjects();

		List<TaskViewModel> models = tasks.Select(task => new TaskViewModel(task, subjects.Find(s => s.Id == task.Homework.SubjectId).Name)).ToList();

		Tasks.Clear();
		models.ForEach(model => Tasks.Add(model));
	}

	[RelayCommand]
	private async Task CreateTask()
	{
		await Shell.Current.GoToAsync("CreateTaskPage");
	}
}
