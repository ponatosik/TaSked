using CommunityToolkit.Mvvm.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.AccessControl;
using TaSked.Api.ApiClient;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

public partial class SubjectsViewModel : ReactiveObject, IActivatableViewModel
{
	private readonly SubjectDataSource _dataSource;
	public ViewModelActivator Activator { get; } = new();

	private ReadOnlyObservableCollection<SubjectViewModel> _subjects;
	public ReadOnlyObservableCollection<SubjectViewModel> Subjects => _subjects;

	private bool _isRefreshing;
	public bool IsRefreshing
	{
		get => this._isRefreshing;
		set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

	[RelayCommand]
	private async Task RefreshAsync()
	{
		await _dataSource.ForceUpdateAsync();
		IsRefreshing = false;
	}

	public SubjectsViewModel(SubjectDataSource dataSource)
	{
		_dataSource = dataSource;
		_dataSource.SubjectSource
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out _subjects)
			.Subscribe();

		this.RaisePropertyChanged(nameof(Subjects));

		// TODO: Unsubscribe from update when view is inactive

		//this.WhenActivated(dispose =>
		//{
		//	_dataSource.SubjectSource
		//		.Connect()
		//		//.SkipInitial()
		//		.ObserveOn(RxApp.MainThreadScheduler)
		//		.Bind(out _subjects)
		//		.Subscribe()
		//		.DisposeWith(dispose);

		//	this.RaisePropertyChanged(nameof(Subjects));
		//});

	}

	[RelayCommand]
	private async Task CreateSubject()
	{
		await Shell.Current.GoToAsync("CreateSubjectPage");
	}
}

