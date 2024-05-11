using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.App.Common;
using TaSked.Application;
using TaSked.Domain;
using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class SortBySubjViewModel : ObservableObject
{
	private readonly ITaSkedSubjects _subjectService;
	private readonly HomeworkTasksService _tasksService;

    [ObservableProperty]
    private ObservableCollection<TaskGroupModel> _taskGroups;
    [ObservableProperty]
    bool isRefreshing;

    public SortBySubjViewModel(ITaSkedSubjects subjectService, HomeworkTasksService taskService)
	{
		_subjectService = subjectService;
		_tasksService = taskService;
        _taskGroups = new ObservableCollection<TaskGroupModel>();
        LoadTasks();
	}


    [RelayCommand]
    async Task RefreshAsync()
    {
        LoadTasks();
        IsRefreshing = false;
    }

    private async Task LoadTasks()
	{
        List<HomeworkTask> tasks = new List<HomeworkTask>();
        List<SubjectDTO> subjects = new List<SubjectDTO>();

        tasks = await _tasksService.GetAllAsync();
        subjects = await _subjectService.GetAllSubjects();

        List<TaskViewModel> models = tasks
            .Select(task => new TaskViewModel(task, subjects.Find(s => s.Id == task.Homework.SubjectId).Name))
            .OrderBy(task => task.SubjectName)
            .ToList();

        List<TaskGroupModel> groups = models
            .GroupBy(model => model.SubjectName)
            .Select(grouping => new TaskGroupModel(grouping.Key?.ToString() ?? "no subject", grouping.ToList()))
            .ToList();

        TaskGroups.Clear();
        groups.ForEach(group => TaskGroups.Add(group));
    }

	[RelayCommand]
	private async Task CreateTask()
	{
		await Shell.Current.GoToAsync("CreateTaskPage");
	}
}
