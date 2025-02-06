using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.Domain;

namespace TaSked.App;

[QueryProperty(nameof(Homework), "homework")]
public partial class UpdateTaskViewModel : ObservableObject
{
    private ITaSkedHomeworks _homeworkService;

    [ObservableProperty]
    private Homework _homework;

    [ObservableProperty]
    public IReactiveCommand _updateTaskCommand;

    public UpdateTaskViewModel(ITaSkedHomeworks homeworkServic)
    {
        UpdateTaskCommand = ReactiveCommand.CreateFromTask(UpdateTask);

        _homeworkService = homeworkServic;
    }

    private async Task UpdateTask()
    {
        PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var changeTitleRequest = new ChangeHomeworkTitleRequest(Homework.Title);
			await _homeworkService.ChangeHomeworkTitle(changeTitleRequest, Homework.SubjectId, Homework.Id);

			var changeDescriptionRequest = new ChangeHomeworkDescriptionRequest(Homework.Description);
			await _homeworkService.ChangeHomeworkDescription(changeDescriptionRequest, Homework.SubjectId, Homework.Id);

			var changeDeadlineRequest = new ChangeHomeworkDeadlineRequest(Homework.Deadline);
			await _homeworkService.ChangeHomeworkDeadline(changeDeadlineRequest, Homework.SubjectId, Homework.Id);
		});

		await Shell.Current.GoToAsync("..");
    }
}
