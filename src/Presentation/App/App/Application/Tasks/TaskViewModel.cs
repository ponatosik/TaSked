using DynamicData;
using LocalizationResourceManager.Maui;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.Domain;

namespace TaSked.App;

public partial class TaskViewModel : ReactiveObject
{
    private HomeworkTask _task;
    public HomeworkTask Task
    {
        get => _task;
        set => this.RaiseAndSetIfChanged(ref _task, value);
    }

    private string _subjectName;
    public string SubjectName
    {
        get => _subjectName;
        set => this.RaiseAndSetIfChanged(ref _subjectName, value);
    }

    private Color _strokeColor;
    public Color StrokeColor
    {
        get => _strokeColor;
        set => this.RaiseAndSetIfChanged(ref _strokeColor, value);
    }

    public TaskViewModel(HomeworkTask task, string subjectName)
    {
        Task = task;
        SubjectName = subjectName;
        if(Task.MetaData == null)
        {
            StrokeColor = Color.Parse("#49454F");
        }
        else
        {
            StrokeColor = Color.Parse(task.MetaData);
        }

        CompleteCommand = ReactiveCommand.CreateFromTask(Complete);
        StrokeColorCommand = ReactiveCommand.CreateFromTask(StrokeChangeColor);
        UndoCompletionCommand = ReactiveCommand.CreateFromTask(UndoCompletion);
        UpdateHomeworkCommand = ReactiveCommand.CreateFromTask(UpdateHomework);
        DeleteHomeworkCommand = ReactiveCommand.CreateFromTask(DeleteHomework);
    }

    private IReactiveCommand _completedCommand;
    public IReactiveCommand CompleteCommand
    {
        get => _completedCommand;
        set => this.RaiseAndSetIfChanged(ref _completedCommand, value);
    }

    private IReactiveCommand _strokeColorCommand;
    public IReactiveCommand StrokeColorCommand
    {
        get => _strokeColorCommand;
        set => this.RaiseAndSetIfChanged(ref _strokeColorCommand, value);
    }

    private IReactiveCommand _undoCompletionCommand;
    public IReactiveCommand UndoCompletionCommand
    {
        get => _undoCompletionCommand;
        set => this.RaiseAndSetIfChanged(ref _undoCompletionCommand, value);
    }

    private IReactiveCommand _updateHomeworkCommand;
    public IReactiveCommand UpdateHomeworkCommand
    {
        get => _updateHomeworkCommand;
        set => this.RaiseAndSetIfChanged(ref _updateHomeworkCommand, value);
    }

    private IReactiveCommand _deleteHomeworkCommand;
    public IReactiveCommand DeleteHomeworkCommand
    {
        get => _deleteHomeworkCommand;
        set => this.RaiseAndSetIfChanged(ref _deleteHomeworkCommand, value);
    }

    private async Task Complete()
    {
        HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
        await tasksService.CompleteAsync(Task);
        ServiceHelper.Services.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(this);
    }

    private async Task StrokeChangeColor()
    {
        ILocalizationResourceManager localization = ServiceHelper.GetService<ILocalizationResourceManager>();

        var colors = new Dictionary<string, string>
        {
            [localization["Task.Action.ChangeColor.Color.White"]] =  "White",
            [localization["Task.Action.ChangeColor.Color.Red"]] = "Red",
            [localization["Task.Action.ChangeColor.Color.Green"]] = "Green",
            [localization["Task.Action.ChangeColor.Color.Yellow"]] = "Yellow",
            [localization["Task.Action.ChangeColor.Color.Purple"]] = "Purple",
            [localization["Task.Action.ChangeColor.Color.Orange"]] = "Orange"
        };

        string actionSheetTile = localization["Task.Action.ChangeColor.Title"];
        string actionSheetCancel = localization["Task.Action.ChangeColor.Cancel"];

        string selectedColor = await Shell.Current.DisplayActionSheet(actionSheetTile, actionSheetCancel, null, colors.Keys.ToArray());

        if (selectedColor != null)
        {
            StrokeColor = Color.Parse(colors[selectedColor]);
            Task.MetaData = colors[selectedColor];
            await ServiceHelper.GetService<HomeworkTasksService>().UpdateStrokeColorAsync(Task);
        }
    }

    private async Task UndoCompletion()
    {
        HomeworkTasksService tasksService = ServiceHelper.GetService<HomeworkTasksService>();
        await tasksService.UndoCompletionAsync(Task);
        ServiceHelper.Services.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(this);
    }

    private async Task UpdateHomework()
    {
        await Shell.Current.GoToAsync("UpdateTaskPage", new Dictionary<string, object>
        {
            ["homework"] = Task.Homework
        });
    }

    private async Task DeleteHomework()
    {
        PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
        await popup.IndicateTaskRunningAsync(async () =>
        {
            ITaSkedHomeworks api = ServiceHelper.GetService<ITaSkedHomeworks>();
            DeleteHomeworkRequest request = new DeleteHomeworkRequest(Task.Homework.SubjectId, Task.Homework.Id);
            await api.DeleteHomework(request);

            HomeworkDataSource homeworkSource = ServiceHelper.GetService<HomeworkDataSource>();
            homeworkSource.HomeworkSource.Remove(Task.Homework.Id);
        });
    }
}
