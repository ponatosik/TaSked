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
using DynamicData.Binding;

namespace TaSked.App;

public partial class UncompletedTasksViewModel : ReactiveObject, IActivatableViewModel
{
	//private readonly ITaSkedSubjects _subjectService;
	//private readonly HomeworkTasksService _tasksService;

	private readonly HomeworkDataSource _dataSource;

	private ReadOnlyObservableCollection<TaskGroupModel> _taskGroups;
	public ReadOnlyObservableCollection<TaskGroupModel> TaskGroups	{
		get => _taskGroups;
		set => this.RaiseAndSetIfChanged(ref _taskGroups, value);
	}

  private bool _isRefreshing;
	public bool IsRefreshing 
	{ 
		get => this._isRefreshing;
    set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

	public ViewModelActivator Activator { get; } = new ();


	// TODO: Use dynamic filter and use same group data source in all groups
	private Func<TaskViewModel, bool> _filter = task => !task.Task.Completed;
	public Func<TaskViewModel, bool> Filter
	{
		get => _filter;
		set => this.RaiseAndSetIfChanged(ref _filter, value);
	}

	private IComparer<TaskViewModel> _sort = Comparer<TaskViewModel>.Create((t1, t2) => t1.SubjectName.CompareTo(t2.SubjectName));
	public IComparer<TaskViewModel> Sort{
    get => _sort;
		set => this.RaiseAndSetIfChanged(ref _sort, value);
	}

    [RelayCommand]
    async Task RefreshAsync()
    {
		await _dataSource.ForceUpdateAsync();
		IsRefreshing = false;
    }

	public UncompletedTasksViewModel(HomeworkDataSource dataSource)
	{
		//_subjectService = subjectService;
		//_tasksService = taskService;
		_dataSource = dataSource;

		var sort = this.WhenAnyValue(x => x.Sort);
		var filter = this.WhenAnyValue(x => x.Filter);

		_dataSource.HomeworkSource
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Group(task => (task.Task.Homework.Deadline)?.ToString("MM.dd.yyyy, dddd") ?? "До кінця семестру")
			.Transform(group => new TaskGroupModel(group, filter, sort))
			.SortBy(group => group.Title)
			.Bind(out _taskGroups)
			.Subscribe();

		this.RaisePropertyChanged(nameof(_taskGroups));
	}

	[RelayCommand]
	private async Task CreateTask()
	{
		await Shell.Current.GoToAsync("CreateTaskPage");
	}
}
