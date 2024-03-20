using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

public partial class SubjectsViewModel : ObservableObject
{
	private readonly ITaSkedSevice _api;

	[ObservableProperty]
	private ObservableCollection<SubjectViewModel> _subjects;

	public SubjectsViewModel(ITaSkedSevice api)
	{
		_api = api;
		_subjects = new ObservableCollection<SubjectViewModel>();
	}

	[RelayCommand]
	public void ReloadSubjects()
	{ 	
		Subjects.Clear();
		LoadSubjects().ForEach(subject => Subjects.Add(new SubjectViewModel(subject)));
	}

	private List<SubjectDTO> LoadSubjects()
	{
		return _api.GetAllSubjects().Result;
	}

	[RelayCommand]
	private async Task CreateSubject()
	{
		await Shell.Current.GoToAsync("CreateSubjectPage");		
	}
}

