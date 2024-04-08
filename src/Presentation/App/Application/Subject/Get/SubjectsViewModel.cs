using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

public partial class SubjectsViewModel : ObservableObject
{
	private readonly ITaSkedSubjects _subjectService;

	[ObservableProperty]
	private ObservableCollection<SubjectViewModel> _subjects;

	public SubjectsViewModel(ITaSkedSubjects subjectService)
	{
		_subjectService = subjectService;
		_subjects = new ObservableCollection<SubjectViewModel>();
	}

	[RelayCommand]
	public async Task ReloadSubjects()
	{ 	
		Subjects.Clear();
		(await LoadSubjects()).ForEach(subject => Subjects.Add(new SubjectViewModel(subject)));
	}

	private async Task<List<SubjectDTO>> LoadSubjects()
	{
		return await _subjectService.GetAllSubjects();
	}

	[RelayCommand]
	private async Task CreateSubject()
	{
		await Shell.Current.GoToAsync("CreateSubjectPage");		
	}
}

