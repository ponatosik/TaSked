using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;

namespace TaSked.App;

public partial class CreateReportViewModel : ObservableObject
{
	private readonly ITaSkedReports _api;
	private readonly ReportDataSource _dataSource;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private string _message;

	public CreateReportViewModel(ITaSkedReports api, ReportDataSource dataSource)
	{
		_api = api;
		_dataSource = dataSource;
	}

	[RelayCommand]
	private async Task CreateReport()
	{
		if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Message)) 
		{
			return;
		}

		var request = new CreateReportRequest(Title, Message);
		var report = await _api.CreateReport(request);
		await Shell.Current.GoToAsync("..");
		_dataSource.ReportSource.AddOrUpdate(report);
	}
}
