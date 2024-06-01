using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.App.Common;
using DynamicData;
using ReactiveUI;
using System.Reactive.Linq;
using TaSked.App.Common.Components;

namespace TaSked.App;

public partial class CreateTaskViewModel : ObservableObject
{
	private readonly ITaSkedHomeworks _homeworkService;
	private readonly SubjectDataSource _subjectSource;

	[ObservableProperty]
	private SubjectDTO _subject;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private DateTime? _deadline = DateTime.Now;

	[ObservableProperty]
	private string _description = string.Empty;

	[ObservableProperty]
	private ReadOnlyObservableCollection<SubjectDTO> _availableSubjects;

	[ObservableProperty]
	private IReactiveCommand _createTaskCommand;

	public CreateTaskViewModel(ITaSkedHomeworks homeworkService, SubjectDataSource subjectSource)
	{
		_homeworkService = homeworkService;
		_subjectSource = subjectSource;

		CreateTaskCommand = ReactiveCommand.CreateFromTask(CreateTask);

		subjectSource.SubjectSource
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Transform(viewModel => viewModel.SubjectDTO)
			.Bind(out _availableSubjects)
			.Subscribe();

		OnPropertyChanged(nameof(AvailableSubjects));
	}

	private async Task CreateTask()
	{
		if (string.IsNullOrEmpty(Title) || Subject is null)
		{
			return;
		}

		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var request = new CreateHomeworkRequest(Subject.Id, Title, Description, Deadline);
			var homework = await _homeworkService.CreateHomework(request);
			await Shell.Current.GoToAsync("..");

			TaskViewModel viewModel = new TaskViewModel(homework.CreateTask(), Subject.Name);
			ServiceHelper.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(viewModel);
		});
	}
}
