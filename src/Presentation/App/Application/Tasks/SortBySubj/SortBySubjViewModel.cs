using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.App.Common;
using TaSked.Application;
using TaSked.Domain;
using TaSked.Api.ApiClient;
using ReactiveUI;
using System.Reactive.Linq;
using DynamicData;

namespace TaSked.App;

public partial class SortBySubjViewModel : ReactiveObject, IActivatableViewModel
{
	private readonly HomeworkDataSource _dataSource;

	private ReadOnlyObservableCollection<TaskGroupModel> _taskGroups;
	public ReadOnlyObservableCollection<TaskGroupModel> TaskGroups
	{
		get => _taskGroups;
		set => this.RaiseAndSetIfChanged(ref _taskGroups, value);
	}

	// TODO: Use dynamic filter and use same group data source in all groups
	private Func<TaskViewModel, bool> _filter = task =>
		!task.Task.Completed ||
		(task.Task.Homework?.Deadline ?? DateTime.MaxValue).Date >= DateTime.Now;

	public Func<TaskViewModel, bool> Filter
	{
		get => _filter;
		set => this.RaiseAndSetIfChanged(ref _filter, value);
	}

	private IComparer<TaskViewModel> _sort = 
		Comparer<TaskViewModel>.Create((t1, t2) => t1.Task.Completed.CompareTo(t2.Task.Completed))
		.ThenBy(t => t.Task.Homework.Deadline);

  private bool _isRefreshing;
	public bool IsRefreshing 
	{ 
		get => this._isRefreshing;
    set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

	public IComparer<TaskViewModel> Sort
	{
		get => _sort;
		set => this.RaiseAndSetIfChanged(ref _sort, value);
	}


    public SortBySubjViewModel(HomeworkDataSource dataSource)	
    {
		_dataSource = dataSource;

		var sort = this.WhenAnyValue(x => x.Sort);
		var filter = this.WhenAnyValue(x => x.Filter);

		_dataSource.HomeworkSource
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Group(task => task.SubjectName)
			.Transform(group => new TaskGroupModel(group, filter, sort))
			.Bind(out _taskGroups)
			.Subscribe();

		  this.RaisePropertyChanged(nameof(_taskGroups));
	    }

    [RelayCommand]
    async Task RefreshAsync()
    {
		await _dataSource.ForceUpdateAsync();
        IsRefreshing = false;
    }


	public ViewModelActivator Activator { get; set; } = new ViewModelActivator();

	//private async Task LoadTasks()
	//{
	//       List<HomeworkTask> tasks = new List<HomeworkTask>();
	//       List<SubjectDTO> subjects = new List<SubjectDTO>();

	//       tasks = await _tasksService.GetAllAsync();
	//       subjects = await _subjectService.GetAllSubjects();

	//       List<TaskViewModel> models = tasks
	//           .Select(task => new TaskViewModel(task, subjects.Find(s => s.Id == task.Homework.SubjectId).Name))
	//           .OrderBy(task => task.SubjectName)
	//           .ToList();

	//       List<TaskGroupModel> groups = models
	//           .GroupBy(model => model.SubjectName)
	//           .Select(grouping => new TaskGroupModel(grouping.Key?.ToString() ?? "no subject", grouping.ToList()))
	//           .ToList();

	//       TaskGroups.Clear();
	//       groups.ForEach(group => TaskGroups.Add(group));
	//   }

	[RelayCommand]
	private async Task CreateTask()
	{
		await Shell.Current.GoToAsync("CreateTaskPage");
	}
}
