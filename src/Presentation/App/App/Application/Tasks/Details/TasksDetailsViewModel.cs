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

[QueryProperty(nameof(TaskViewModel), "task")]
public partial class TasksDetailsViewModel : ObservableObject
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	
	private ITaSkedHomeworks _homeworkService;
	
	[ObservableProperty]
	private TaskViewModel _taskViewModel;
    
	[ObservableProperty]
    private IReactiveCommand _deleteHomeworkCommand;
    
    public TasksDetailsViewModel(ITaSkedHomeworks homeworkService, ILocalizationResourceManager localizationResourceManager)
    {
	    DeleteHomeworkCommand = ReactiveCommand.CreateFromTask(DeleteHomework);

	    _homeworkService = homeworkService;
	    _localizationResourceManager = localizationResourceManager;
    }

    private async Task DeleteHomework()
    {
        PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
        await popup.IndicateTaskRunningAsync(async () =>
        {
            ITaSkedHomeworks api = ServiceHelper.GetService<ITaSkedHomeworks>();
            await api.DeleteHomework(TaskViewModel.Task.Homework.SubjectId, TaskViewModel.Task.Homework.Id);

            HomeworkDataSource homeworkSource = ServiceHelper.GetService<HomeworkDataSource>();
            homeworkSource.HomeworkSource.Remove(TaskViewModel.Task.Homework.Id);
        });
        
        await Shell.Current.GoToAsync("..");
    }
}