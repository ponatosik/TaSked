using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;

namespace TaSked.App;

public partial class CreateSubjectViewModel : ObservableObject
{
	private ITaSkedSevice _api;

	[ObservableProperty]
	private string _name;

	public CreateSubjectViewModel(ITaSkedSevice api)
	{
		_api = api;
	}

	[RelayCommand]
	private async Task CreateSubject()
	{
		if (string.IsNullOrEmpty(Name)) 
		{
			return;
		}

		var request = new CreateSubjectRequest(Name);
		var dto = await _api.CreateSubject(request);
		await Shell.Current.GoToAsync("..");

		SubjectViewModel viewModel = new SubjectViewModel(dto);
		var subjectsView = ServiceHelper.GetService<SubjectsViewModel>();
		subjectsView.Subjects.Add(viewModel);
	}
}
