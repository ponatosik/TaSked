using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;

namespace TaSked.App;

public partial class CreateAnnouncementViewModel : ObservableObject
{
	private readonly ITaSkedAnnouncements _api;
	private readonly AnnouncementDataSource _dataSource;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private string _message;

	public CreateAnnouncementViewModel(ITaSkedAnnouncements api, AnnouncementDataSource dataSource)
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

		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var request = new CreateAnnouncementRequest(Title, Message);
			var report = await _api.CreateAnnouncement(request);
			_dataSource.AnnouncementSource.AddOrUpdate(report);
		});

		await Shell.Current.GoToAsync("..");
	}
}
