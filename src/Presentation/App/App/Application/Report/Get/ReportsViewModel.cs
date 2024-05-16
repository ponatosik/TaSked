using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive.Linq;
using TaSked.Api.ApiClient;
using TaSked.Domain;

namespace TaSked.App;

public partial class ReportsViewModel : ReactiveObject, IActivatableViewModel
{
	private readonly ReportDataSource _dataSource;
	public ViewModelActivator Activator { get; } = new();

	private ReadOnlyObservableCollection<Report> _reports;
	public ReadOnlyObservableCollection<Report> Reports => _reports;

	private bool _isRefreshing;
	public bool IsRefreshing
	{
		get => this._isRefreshing;
		set => this.RaiseAndSetIfChanged(ref _isRefreshing, value);
	}

    public ReportsViewModel(ReportDataSource dataSource)
	{
		_dataSource = dataSource;
		_dataSource.ReportSource
			.Connect()
			.SortBy(report => report.Id)
			.ObserveOn(RxApp.MainThreadScheduler)
			.Bind(out _reports)
			.Subscribe();

		this.RaisePropertyChanged(nameof(Reports));
	}

    [RelayCommand]
    private async Task RefreshAsync()
    {
		await _dataSource.ForceUpdateAsync();
		IsRefreshing = false;
    }


	[RelayCommand]
	private async Task CreateReport()
	{
		await Shell.Current.GoToAsync("CreateReportPage");		
	}
}

