using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using TaSked.Domain;

namespace TaSked.App;

public partial class AnnouncementViewModel : ReactiveObject, IActivatableViewModel
{
	private readonly AnnouncementDataSource _dataSource;
	public ViewModelActivator Activator { get; } = new();

	private readonly ReadOnlyObservableCollection<Announcement> _announcements;
	public ReadOnlyObservableCollection<Announcement> Announcements => _announcements;

	private bool _isRefreshing;
	public bool IsRefreshing
	{
		get => this._isRefreshing;
		set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

	public AnnouncementViewModel(AnnouncementDataSource dataSource)
	{
		_dataSource = dataSource;
		_dataSource.AnnouncementSource
			.Connect()
			.SortBy(report => report.Id)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out _announcements)
			.Subscribe();

		RefreshCommand = ReactiveCommand.CreateFromTask(RefreshAsync);

		this.RaisePropertyChanged(nameof(Announcements));
	}

	private IReactiveCommand _refreshCommand;
	public IReactiveCommand RefreshCommand
	{
		get => _refreshCommand;
		set => this.RaiseAndSetIfChanged(ref _refreshCommand, value);
	}

    private async Task RefreshAsync()
    {
		await _dataSource.ForceUpdateAsync();
		IsRefreshing = false;
    }


	[RelayCommand]
	private async Task CreateReport()
	{
		await Shell.Current.GoToAsync("CreateAnnouncementPage");		
	}
}

