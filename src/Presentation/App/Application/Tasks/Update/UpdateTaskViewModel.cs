using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.Domain;

namespace TaSked.App;

[QueryProperty(nameof(Homework), "homework")]
public partial class UpdateTaskViewModel : ObservableObject
{
    private ITaSkedHomeworks _homeworkService;

    [ObservableProperty]
    private Homework _homework;

    public UpdateTaskViewModel(ITaSkedHomeworks homeworkServic)
    {
        _homeworkService = homeworkServic;
    }

    [RelayCommand]
    private async Task UpdateTask()
    {
        var changeTitleRequest = new ChangeHomeworkTitleRequest(Homework.SubjectId, Homework.Id, Homework.Title);
        await _homeworkService.ChangeTitle(changeTitleRequest);

        var changeDescriptionRequest = new ChangeHomeworkDescriptionRequest(Homework.SubjectId, Homework.Id, Homework.Description);
        await _homeworkService.ChangeDescription(changeDescriptionRequest);

        var changeDeadlineRequest = new ChangeHomeworkDeadlineRequest(Homework.SubjectId, Homework.Id, Homework.Deadline);
        await _homeworkService.ChangeDeadline(changeDeadlineRequest);

        await Shell.Current.GoToAsync("..");
    }
}
