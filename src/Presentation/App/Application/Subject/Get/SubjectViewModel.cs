using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.Application;

namespace TaSked.App;

public partial class SubjectViewModel : ObservableObject
{
	[ObservableProperty]
	private SubjectDTO _subjectDTO;

	public SubjectViewModel(SubjectDTO subjectDTO)
	{
		_subjectDTO = subjectDTO;
	}
	public SubjectViewModel() { }

	[RelayCommand]
	private async Task UpdateSubject()
	{
		await Shell.Current.GoToAsync("UpdateSubjectPage", new Dictionary<string, object>
		{
			["subject"] = SubjectDTO
		});	
	}

	[RelayCommand]
	private async Task DeleteSubject()
	{
		ITaSkedSevice api = ServiceHelper.GetService<ITaSkedSevice>();
		SubjectsViewModel viewModel = ServiceHelper.GetService<SubjectsViewModel>();

		var request = new DeleteSubjectRequest(SubjectDTO.Id);
		await api.DeleteSubject(request);

		viewModel.Subjects.Remove(this);
	}
}
