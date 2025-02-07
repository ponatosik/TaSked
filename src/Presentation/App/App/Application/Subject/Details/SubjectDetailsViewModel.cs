using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using LocalizationResourceManager.Maui;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.Application;

namespace TaSked.App;

[QueryProperty(nameof(SubjectDTO), "subject")]
public partial class SubjectDetailsViewModel : ObservableObject
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	
	private ITaSkedSubjects _subjectService;

	[ObservableProperty]
	private SubjectDTO _subjectDTO;

	[ObservableProperty]
	private IReactiveCommand _updateSubjectCommand;
	
	[ObservableProperty]
	private IReactiveCommand _deleteSubjectCommand;

	public SubjectDetailsViewModel(ITaSkedSubjects subjectService, ILocalizationResourceManager localizationResourceManager)
	{
		_subjectService = subjectService;
		_localizationResourceManager = localizationResourceManager;

		UpdateSubjectCommand = ReactiveCommand.CreateFromTask(UpdateSubject);
		DeleteSubjectCommand = ReactiveCommand.CreateFromTask(DeleteSubject);
	}

	private async Task UpdateSubject()
	{
		await Shell.Current.GoToAsync("../UpdateSubjectPage", new Dictionary<string, object>
		{
			["subject"] = SubjectDTO
		});	
	}
	
	private async Task DeleteSubject()
	{
		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			ITaSkedSubjects api = ServiceHelper.GetService<ITaSkedSubjects>();
			await api.DeleteSubject(SubjectDTO.Id);

			SubjectDataSource subjectSource = ServiceHelper.GetService<SubjectDataSource>();
			subjectSource.SubjectSource.Remove(SubjectDTO.Id);
		});
		
		await Shell.Current.GoToAsync("..");
	}

	public string TeacherOnlineLessonsUrlDisplay =>
		string.Join("\n", SubjectDTO.Teachers.Select(t => t.OnlineMeetingUrl?.ToString()));
}