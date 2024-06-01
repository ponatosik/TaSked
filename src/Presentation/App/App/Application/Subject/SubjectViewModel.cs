using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.Application;

namespace TaSked.App;

public partial class SubjectViewModel : ReactiveObject
{
	private SubjectDTO _subjectDTO;
	public SubjectDTO SubjectDTO
	{
		get => _subjectDTO;
		set => this.RaiseAndSetIfChanged(ref _subjectDTO, value);
	}

	private IReactiveCommand _deleteSubjectCommand;
	public IReactiveCommand DeleteSubjectCommand
	{
		get => _deleteSubjectCommand;
		set => this.RaiseAndSetIfChanged(ref _deleteSubjectCommand, value);
	}

	public SubjectViewModel(SubjectDTO subjectDTO)
	{
		_subjectDTO = subjectDTO;

		DeleteSubjectCommand = ReactiveCommand.CreateFromTask(DeleteSubject);
	}

	[RelayCommand]
	private async Task UpdateSubject()
	{
		await Shell.Current.GoToAsync("UpdateSubjectPage", new Dictionary<string, object>
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
			var request = new DeleteSubjectRequest(SubjectDTO.Id);
			await api.DeleteSubject(request);

			SubjectDataSource subjectSource = ServiceHelper.GetService<SubjectDataSource>();
			subjectSource.SubjectSource.Remove(SubjectDTO.Id);
		});
	}
}
