using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.App.Common.Models;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App;

[QueryProperty(nameof(SubjectDTO), "subject")]
public partial class UpdateSubjectViewModel : ObservableObject
{
	private ITaSkedSubjects _subjectService;

	[ObservableProperty] private SubjectDTO _subjectDTO;

	[ObservableProperty] private ObservableCollection<UpdateTeacherDTO> _teachers;

	[ObservableProperty]
	private ObservableCollection<RelatedLinkModel> _relatedLinkInputs;

	public UpdateSubjectViewModel(ITaSkedSubjects subjectService)
	{
		_subjectService = subjectService;
		_teachers = [];
		_relatedLinkInputs = [];
	}

	[RelayCommand]
	private async Task UpdateSubject()
	{
		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var changeNameRequest = new ChangeSubjectNameRequest(SubjectDTO.Name);
			SubjectDTO.Name = (await _subjectService.ChangeSubjectName(changeNameRequest, SubjectDTO.Id)).Name;

			try
			{
				var changeRelatedLinkRequest = new ChangeSubjectLinksRequest(
					RelatedLinkInputs.Select(model => RelatedLink.Create(new Uri(model.Url), model.Title)).ToList());

				SubjectDTO.RelatedLinks =
					(await _subjectService.ChangeSubjectLinks(changeRelatedLinkRequest, SubjectDTO.Id))
					.RelatedLinks;
			}
			catch (UriFormatException ex)
			{
				await Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "OK");
			}

			var changeTeacherRequest = new ChangeSubjectTeachersRequest(
				Teachers.ToList());

			SubjectDTO.Teachers = (await _subjectService.ChangeSubjectTeacher(changeTeacherRequest, SubjectDTO.Id))
				.Teachers;

			SubjectDataSource subjectSource = ServiceHelper.GetService<SubjectDataSource>();
			subjectSource.SubjectSource.AddOrUpdate(new SubjectViewModel(SubjectDTO));
		});

		await Shell.Current.GoToAsync("..");
	}

	[RelayCommand]
	private void RemoveTeacher(UpdateTeacherDTO updateTeacherDto)
	{
		Teachers.Remove(updateTeacherDto);
	}

	[RelayCommand]
	private void AddTeacher()
	{
		Teachers.Add(new UpdateTeacherDTO("", null, null, null, null));
	}

	[RelayCommand]
	private void RemoveRelatedLink(RelatedLinkModel relatedLinkModelInput)
	{
		RelatedLinkInputs.Remove(relatedLinkModelInput);
	}

	[RelayCommand]
	private void AddRelatedLink()
	{
		RelatedLinkInputs.Add(new RelatedLinkModel());
	}
}