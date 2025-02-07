using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.Domain;

namespace TaSked.App;

public partial class CreateSubjectViewModel : ObservableObject
{
    private readonly ITaSkedSubjects _subjectService;
    private readonly SubjectDataSource _subjectDataSource;

    [ObservableProperty]
    public IReactiveCommand _createSubjectCommand;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _teacherName;

    public CreateSubjectViewModel(ITaSkedSubjects subjectService, SubjectDataSource subjectDataSource)
    {
        _subjectService = subjectService;
        _subjectDataSource = subjectDataSource;

        CreateSubjectCommand = ReactiveCommand.CreateFromTask(CreateSubject);
    }

    private async Task CreateSubject()
    {
	    if (string.IsNullOrEmpty(Name))
        {
            return;
        }

        PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
        await popup.IndicateTaskRunningAsync(SendApiRequests);
        await Shell.Current.GoToAsync("..");
    }

    private async Task SendApiRequests()
    {
	    var request = new CreateSubjectRequest(Name);
	    var dto = await _subjectService.CreateSubject(request);

	    if (!string.IsNullOrEmpty(TeacherName))
	    {
		    var teacherDto = new UpdateTeacherDTO(TeacherName, null, null, null, null);
		    var changeTeacherRequest = new ChangeSubjectTeachersRequest([teacherDto]);
		    var updateDto = await _subjectService.ChangeSubjectTeacher(changeTeacherRequest, dto.Id);
		    dto.Teachers = updateDto.Teachers;
	    }

	    var viewModel = new SubjectViewModel(dto);
	    _subjectDataSource.SubjectSource.AddOrUpdate(viewModel);
    }
}
