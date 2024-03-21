using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

public partial class CreateTaskViewModel : ObservableObject
{
	private ITaSkedSevice _api;

	[ObservableProperty]
	private SubjectDTO _subject;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private string _description = string.Empty;

	[ObservableProperty]
	private ObservableCollection<SubjectDTO> _availableSubjects;

	public CreateTaskViewModel(ITaSkedSevice api)
	{
		_api = api;
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

		var request = new CreateHomeworkRequest(Subject.Id, Title, Description);
		await _api.CreateHomework(request);
		await Shell.Current.GoToAsync("..");
	}

	private async Task LoadAvailableSubjects()
	{
		var subjects = await _api.GetAllSubjects();
		AvailableSubjects.Clear();
		subjects.ForEach(subject => AvailableSubjects.Add(subject));
	}
}
