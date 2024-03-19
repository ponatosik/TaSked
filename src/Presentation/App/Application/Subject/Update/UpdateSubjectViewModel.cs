using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.App;

[QueryProperty(nameof(SubjectDTO), "subject")]
public partial class UpdateSubjectViewModel : ObservableObject
{
	private ITaSkedSevice _api;

	[ObservableProperty]
	private SubjectDTO _subjectDTO;

	public UpdateSubjectViewModel(ITaSkedSevice api)
	{
		_api = api;
	}

	[RelayCommand]
	private async Task UpdateSubject()
	{
		var changeNameRequest = new ChangeSubjectNameRequest(SubjectDTO.Id, SubjectDTO.Name);
		await _api.ChangeSubjectName(changeNameRequest);

		var changeTeacherRequest = new ChangeSubjectTeacherRequest(SubjectDTO.Id, SubjectDTO.Teacher);
		await _api.ChangeSubjectTeacher(changeTeacherRequest);

		await Shell.Current.GoToAsync("..");
	}
}
