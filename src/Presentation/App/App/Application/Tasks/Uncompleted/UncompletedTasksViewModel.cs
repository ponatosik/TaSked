using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive.Linq;
using DynamicData;
using LocalizationResourceManager.Maui;
using TaSked.App.Common;

namespace TaSked.App;

public partial class UncompletedTasksViewModel : ReactiveObject, IActivatableViewModel
{
	//private readonly ITaSkedSubjects _subjectService;
	//private readonly HomeworkTasksService _tasksService;
	private readonly ILocalizationResourceManager _localizationResourceManager;
		

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

	private IReactiveCommand _refreshCommand;
	public IReactiveCommand RefreshCommand
	{
		get => _refreshCommand;
		set => this.RaiseAndSetIfChanged(ref _refreshCommand, value);
	}

    async Task RefreshAsync()
    {
		await _dataSource.ForceUpdateAsync();
		IsRefreshing = false;
    }

	public UncompletedTasksViewModel(HomeworkDataSource dataSource, ILocalizationResourceManager localizationResourceManager)
	{
		//_subjectService = subjectService;
		//_tasksService = taskService;
		_dataSource = dataSource;
		_localizationResourceManager = localizationResourceManager;

		var sort = this.WhenAnyValue(x => x.Sort);
		var filter = this.WhenAnyValue(x => x.Filter);

		RefreshCommand = ReactiveCommand.CreateFromTask(RefreshAsync);

		_dataSource.HomeworkSource
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Group(task => (task.Task.Homework.Deadline)?.ToString("MM.dd.yyyy, dddd") ?? _localizationResourceManager["Task_Deadline_Title"])
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
