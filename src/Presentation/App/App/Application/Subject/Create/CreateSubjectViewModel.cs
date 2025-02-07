using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;

namespace TaSked.App;

public partial class CreateSubjectViewModel : ObservableObject
{
    private readonly ITaSkedSubjects _subjectService;
    private readonly SubjectDataSource _subjectDataSource;

    [ObservableProperty]
    public IReactiveCommand _createSubjectCommand;

    [ObservableProperty]
    private string _name;

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
        await popup.IndicateTaskRunningAsync(async () =>
        {
	        var request = new CreateSubjectRequest(Name);
            var dto = await _subjectService.CreateSubject(request);

            SubjectViewModel viewModel = new SubjectViewModel(dto);
            _subjectDataSource.SubjectSource.AddOrUpdate(viewModel);
        });

        await Shell.Current.GoToAsync("..");
    }
}
