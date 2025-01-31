using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using LocalizationResourceManager.Maui;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.Domain;

namespace TaSked.App;

[QueryProperty(nameof(Homework), "homework")]
public partial class TasksDetailsViewModel : ObservableObject
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	
	private ITaSkedHomeworks _homeworkService;

	[ObservableProperty]
	private Homework _homework;
	
	[ObservableProperty]
    private IReactiveCommand _updateHomeworkCommand;
    
	[ObservableProperty]
    private IReactiveCommand _deleteHomeworkCommand;
    
    public TasksDetailsViewModel(ITaSkedHomeworks homeworkService, ILocalizationResourceManager localizationResourceManager)
    {
	    UpdateHomeworkCommand = ReactiveCommand.CreateFromTask(UpdateHomework);
	    DeleteHomeworkCommand = ReactiveCommand.CreateFromTask(DeleteHomework);

	    _homeworkService = homeworkService;
	    _localizationResourceManager = localizationResourceManager;
    }
    
    private async Task UpdateHomework()
    {
        await Shell.Current.GoToAsync("UpdateTaskPage", new Dictionary<string, object>
        {
            ["homework"] = Homework
        });
    }

    private async Task DeleteHomework()
    {
        PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
        await popup.IndicateTaskRunningAsync(async () =>
        {
            ITaSkedHomeworks api = ServiceHelper.GetService<ITaSkedHomeworks>();
            DeleteHomeworkRequest request = new DeleteHomeworkRequest(Homework.SubjectId, Homework.Id);
            await api.DeleteHomework(request);

            HomeworkDataSource homeworkSource = ServiceHelper.GetService<HomeworkDataSource>();
            homeworkSource.HomeworkSource.Remove(Homework.Id);
        });
        
        await Shell.Current.GoToAsync("..");
    }

    public string LessonUrlDisplay =>
	    Homework?.RelatedLinks.Count != 0
		    ? string.Join(" ", Homework!.RelatedLinks.Select(link => $"{link.Title}: {link:Url}"))
		    : _localizationResourceManager["Details_None"];
}