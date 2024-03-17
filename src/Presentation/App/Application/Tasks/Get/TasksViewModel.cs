using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

public partial class TasksViewModel : ObservableObject
{
	private readonly ITaSkedSevice _api;

	[ObservableProperty]
	private ObservableCollection<TaskViewModel> _tasks;

	public TasksViewModel(ITaSkedSevice api)
	{
		_api = api;
		_tasks = new ObservableCollection<TaskViewModel>(LoadTasks());
	}

	private List<TaskViewModel> LoadTasks()
	{
		List<Homework> homeworks = new List<Homework>();
		List<SubjectDTO> subjects = new List<SubjectDTO>();

		homeworks = _api.GetAllHomework().Result;
		subjects = _api.GetAllSubjects().Result;

		List<HomeworkTask> tasks = homeworks.Select(hw => hw.CreateTask()).ToList();

		List<TaskViewModel> models = tasks.Select(task => new TaskViewModel(task, 
			subjects.Find(s => s.Id == task.Homework.SubjectId).Name)).ToList();

		return models;
	}

	private string GetSubjectName(HomeworkTask task)
	{
		return "Subject";
	}
}
