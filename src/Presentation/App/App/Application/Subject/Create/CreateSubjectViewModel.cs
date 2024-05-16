using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;

namespace TaSked.App;

public partial class CreateSubjectViewModel : ObservableObject
{
	private readonly ITaSkedSubjects _subjectService;
	private readonly SubjectDataSource _subjectDataSource;

	[ObservableProperty]
	private string _name;

	public CreateSubjectViewModel(ITaSkedSubjects subjectService, SubjectDataSource subjectDataSource)
	{
		_subjectService = subjectService;
		_subjectDataSource = subjectDataSource;
	}

	[RelayCommand]
	private async Task CreateSubject()
	{
		if (string.IsNullOrEmpty(Name)) 
		{
			return;
		}

		var request = new CreateSubjectRequest(Name);
		var dto = await _subjectService.CreateSubject(request);
		await Shell.Current.GoToAsync("..");

		SubjectViewModel viewModel = new SubjectViewModel(dto);
		_subjectDataSource.SubjectSource.AddOrUpdate(viewModel);
	}
}
