using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Domain;

namespace TaSked.App;

public partial class RepotrsViewModel : ObservableObject
{
	private readonly ITaSkedReports _reportsService;

	[ObservableProperty]
	private ObservableCollection<Report> _reports;

	public RepotrsViewModel(ITaSkedReports api)
	{
		_reportsService = api;
		_reports = new ObservableCollection<Report>();
	}

	[RelayCommand]
	public async Task ReloadReports()
	{ 	
		Reports.Clear();
		(await LoadReports()).ForEach(report => Reports.Add(report));
	}

	private Task<List<Report>> LoadReports()
	{
		return _reportsService.GetAllReports();
	}

	[RelayCommand]
	private async Task CreateReport()
	{
		await Shell.Current.GoToAsync("CreateReportPage");		
	}
}

