using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.App.Common;
using TaSked.Application;
using TaSked.Domain;
using TaSked.Api.ApiClient;
using ReactiveUI;
using DynamicData;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace TaSked.App;

public partial class AllTasksViewModel : ReactiveObject, IActivatableViewModel
{
	private readonly HomeworkDataSource _dataSource;

	private ReadOnlyObservableCollection<TaskViewModel> _tasks;
	public ReadOnlyObservableCollection<TaskViewModel> Tasks => _tasks;

	public ViewModelActivator Activator { get; } = new();

	public AllTasksViewModel(HomeworkDataSource dataSource)
	{
		_dataSource = dataSource;
		_tasks = new ReadOnlyObservableCollection<TaskViewModel>(new ObservableCollection<TaskViewModel>());

		_dataSource.HomeworkSource
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out _tasks)
			.Subscribe();

		this.RaisePropertyChanged(nameof(Tasks));
		// TODO: Unsubscribe from update when view is inactive

		//this.WhenActivated(dispose =>
		//{
		//	_dataSource.HomeworkSource
		//		.Connect()
		//		.ObserveOn(RxApp.MainThreadScheduler)
		//		.Bind(out _tasks)
		//		.Subscribe()
		//		.DisposeWith(dispose);
		//});  
	}


	private bool _isRefreshing;
	public bool IsRefreshing
	{
		get => this._isRefreshing;
		set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

	[RelayCommand]
	async Task RefreshAsync()
	{
		//TODO: refresh cache
		IsRefreshing = false;
	}


	[RelayCommand]
	private async Task CreateTask()
	{
		await Shell.Current.GoToAsync("CreateTaskPage");
	}
}
