﻿using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.Application;

namespace TaSked.App;

[QueryProperty(nameof(SubjectDTO), "subject")]
public partial class UpdateSubjectViewModel : ObservableObject
{
	private ITaSkedSubjects _subjectService;

	[ObservableProperty]
	private SubjectDTO _subjectDTO;

	[ObservableProperty]
	private IReactiveCommand _updateSubjectCommand;

	public UpdateSubjectViewModel(ITaSkedSubjects subjectService)
	{
		_subjectService = subjectService;

		UpdateSubjectCommand = ReactiveCommand.CreateFromTask(UpdateSubject);
	}

	private async Task UpdateSubject()
	{
		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var changeNameRequest = new ChangeSubjectNameRequest(SubjectDTO.Id, SubjectDTO.Name);
			SubjectDTO.Name = (await _subjectService.ChangeSubjectName(changeNameRequest)).Name ?? SubjectDTO.Name;

			var changeTeacherRequest = new ChangeSubjectTeacherRequest(SubjectDTO.Id, SubjectDTO.Teacher);
			SubjectDTO.Teacher = (await _subjectService.ChangeSubjectTeacher(changeTeacherRequest)).Teacher ?? SubjectDTO.Teacher;

			SubjectDataSource subjectSource = ServiceHelper.GetService<SubjectDataSource>();
			subjectSource.SubjectSource.AddOrUpdate(new SubjectViewModel(SubjectDTO));

		});

		await Shell.Current.GoToAsync("..");
	}
}
