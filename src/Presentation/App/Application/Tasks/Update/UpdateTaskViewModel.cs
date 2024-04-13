using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

[QueryProperty(nameof(Homework), "homework")]
public partial class UpdateTaskViewModel : ObservableObject
{
    private ITaSkedSevice _api;

    [ObservableProperty]
    private Homework _homework;

    public UpdateTaskViewModel(ITaSkedSevice api)
    {
        _api = api;
    }

    [RelayCommand]
    private async Task UpdateTask()
    {
        var changeTitleRequest = new ChangeHomeworkTitleRequest(Homework.SubjectId, Homework.Id, Homework.Title);
        await _api.ChangeTitle(changeTitleRequest);

        var changeDescriptionRequest = new ChangeHomeworkDescriptionRequest(Homework.SubjectId, Homework.Id, Homework.Description);
        await _api.ChangeDescription(changeDescriptionRequest);

        var changeDeadlineRequest = new ChangeHomeworkDeadlineRequest(Homework.SubjectId, Homework.Id, Homework.Deadline);
        await _api.ChangeDeadline(changeDeadlineRequest);

        await Shell.Current.GoToAsync("..");
    }
}
