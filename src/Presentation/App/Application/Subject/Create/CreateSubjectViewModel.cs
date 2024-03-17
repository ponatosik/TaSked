using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;

namespace TaSked.App;

public partial class CreateSubjectViewModel : ObservableObject
{
	private ITaSkedSevice _api;

	[ObservableProperty]
	private string _name;

	[ObservableProperty]
	private string _teacherName;

	public CreateSubjectViewModel(ITaSkedSevice api)
	{
		_api = api;
	}

	[RelayCommand]
	private async Task CreateSubject()
	{
		if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(TeacherName)) 
		{
			return;
		}

		var request = new CreateSubjectRequest(_name);
		await _api.CreateSubject(request);
		await Shell.Current.GoToAsync("..");
	}
}
