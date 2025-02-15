using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.App.Common.Components;
using TaSked.App.Common.Models;
using TaSked.Domain;

namespace TaSked.App;

[QueryProperty(nameof(Homework), "homework")]
public partial class UpdateTaskViewModel : ObservableObject
{
	private ITaSkedHomeworks _homeworkService;

	[ObservableProperty] private Homework _homework;

	[ObservableProperty]
	private ObservableCollection<RelatedLinkModel> _relatedLinkInputs;

	public UpdateTaskViewModel(ITaSkedHomeworks homeworkServic)
	{
		_relatedLinkInputs = [];
		_homeworkService = homeworkServic;
	}

	[RelayCommand]
	private async Task UpdateTask()
	{
		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var changeTitleRequest = new ChangeHomeworkTitleRequest(Homework.Title);
			await _homeworkService.ChangeHomeworkTitle(changeTitleRequest, Homework.SubjectId, Homework.Id);

			var changeDescriptionRequest = new ChangeHomeworkDescriptionRequest(Homework.Description);
			await _homeworkService.ChangeHomeworkDescription(changeDescriptionRequest, Homework.SubjectId, Homework.Id);

			var changeDeadlineRequest = new ChangeHomeworkDeadlineRequest(Homework.Deadline);
			await _homeworkService.ChangeHomeworkDeadline(changeDeadlineRequest, Homework.SubjectId, Homework.Id);
			
			try
			{
				var changeRelatedLinkRequest = new ChangeHomeworkRelatedLinksRequest(
					RelatedLinkInputs.Select(model => RelatedLink.Create(new Uri(model.Url), model.Title)).ToList());

				Homework.RelatedLinks.AddRange(
					(await _homeworkService.ChangeHomeworkSourceUrl(changeRelatedLinkRequest, Homework.SubjectId,
						Homework.Id)).RelatedLinks);
			}
			catch (UriFormatException ex)
			{
				await Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "OK");
			}
		});

		await Shell.Current.GoToAsync("..");
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