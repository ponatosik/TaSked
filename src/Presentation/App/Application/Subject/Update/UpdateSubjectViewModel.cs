using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;

namespace TaSked.App;

[QueryProperty(nameof(SubjectDTO), "subject")]
public partial class UpdateSubjectViewModel : ObservableObject
{
	private ITaSkedSubjects _subjectService;

	[ObservableProperty]
	private SubjectDTO _subjectDTO;

	public UpdateSubjectViewModel(ITaSkedSubjects subjectService)
	{
		_subjectService = subjectService;
	}

	[RelayCommand]
	private async Task UpdateSubject()
	{
		var changeNameRequest = new ChangeSubjectNameRequest(SubjectDTO.Id, SubjectDTO.Name);
		await _subjectService.ChangeSubjectName(changeNameRequest);

		var changeTeacherRequest = new ChangeSubjectTeacherRequest(SubjectDTO.Id, SubjectDTO.Teacher);
		await _subjectService.ChangeSubjectTeacher(changeTeacherRequest);

		await Shell.Current.GoToAsync("..");
	}
}
