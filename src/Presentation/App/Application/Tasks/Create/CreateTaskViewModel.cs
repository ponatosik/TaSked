using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Domain;
using TaSked.App.Common;

namespace TaSked.App;

public partial class CreateTaskViewModel : ObservableObject
{
	private ITaSkedHomeworks _homeworkService;
	private ITaSkedSubjects _subjectService;

	[ObservableProperty]
	private SubjectDTO _subject;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private DateTime? _deadline = DateTime.Now;

	[ObservableProperty]
	private string _description = string.Empty;

	[ObservableProperty]
	private ObservableCollection<SubjectDTO> _availableSubjects;

	public CreateTaskViewModel(ITaSkedHomeworks homeworkService, ITaSkedSubjects subjectService)
	{
		_homeworkService = homeworkService;
		_subjectService = subjectService;
		AvailableSubjects = new ObservableCollection<SubjectDTO>();
		LoadAvailableSubjects();
	}

	[RelayCommand]
	private async Task CreateTask()
	{
		if (string.IsNullOrEmpty(Title) || Subject is null) 
		{
			return;
		}

		var request = new CreateHomeworkRequest(Subject.Id, Title, Description, Deadline);
		var homework = await _homeworkService.CreateHomework(request);
		await Shell.Current.GoToAsync("..");

		TaskViewModel viewModel = new TaskViewModel(homework.CreateTask(), Subject.Name);
		var tasksView = ServiceHelper.GetService<AllTasksViewModel>();
		tasksView.Tasks.Add(viewModel);
	}

	private async Task LoadAvailableSubjects()
	{
		var subjects = await _subjectService.GetAllSubjects();
		AvailableSubjects.Clear();
		subjects.ForEach(subject => AvailableSubjects.Add(subject));
	}
}
