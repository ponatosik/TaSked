using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;

namespace TaSked.App;

public partial class CreateReportViewModel : ObservableObject
{
	private ITaSkedSevice _api;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private string _message;

	public CreateReportViewModel(ITaSkedSevice api)
	{
		_api = api;
	}

	[RelayCommand]
	private async Task CreateReport()
	{
		if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Message)) 
		{
			return;
		}

		var request = new CreateReportRequest(Title, Message);
		await _api.CreateReport(request);
		await Shell.Current.GoToAsync("..");
	}
}
