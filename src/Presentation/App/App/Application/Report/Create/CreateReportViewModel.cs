using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;

namespace TaSked.App;

public partial class CreateReportViewModel : ObservableObject
{
	private readonly ITaSkedReports _api;
	private readonly ReportDataSource _dataSource;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private string _message;

	[ObservableProperty]
	private IReactiveCommand _createReportCommand;

	public CreateReportViewModel(ITaSkedReports api, ReportDataSource dataSource)
	{
		_api = api;
		_dataSource = dataSource;

		CreateReportCommand = ReactiveCommand.CreateFromTask(CreateReport);
	}

	private async Task CreateReport()
	{
		if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Message)) 
		{
			return;
		}

		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var request = new CreateReportRequest(Title, Message);
			var report = await _api.CreateReport(request);
			await Shell.Current.GoToAsync("..");
			_dataSource.ReportSource.AddOrUpdate(report);
		});
	}
}
