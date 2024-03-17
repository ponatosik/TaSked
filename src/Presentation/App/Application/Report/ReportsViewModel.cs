using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Domain;

namespace TaSked.App;

public partial class RepotrsViewModel : ObservableObject
{
	private readonly ITaSkedSevice _api;

	[ObservableProperty]
	private ObservableCollection<Report> _reports;

	public RepotrsViewModel(ITaSkedSevice api)
	{
		_api = api;
		_reports = new ObservableCollection<Report>(LoadReports());
	}

	private List<Report> LoadReports()
	{
		return _api.GetAllReports().Result;
	}

	[RelayCommand]
	private async Task CreateReport()
	{
		await Shell.Current.GoToAsync("CreateReportPage");		
	}
}

