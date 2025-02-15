using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.App.Common;
using DynamicData;
using ReactiveUI;
using System.Reactive.Linq;
using TaSked.App.Common.Components;
using TaSked.Domain;

namespace TaSked.App;

public partial class CreateTaskViewModel : ObservableObject
{
	private readonly ITaSkedHomeworks _homeworkService;
	private readonly SubjectDataSource _subjectSource;

	[ObservableProperty]
	private SubjectDTO _subject;

	[ObservableProperty]
	private string _title;

	[ObservableProperty]
	private DateTime? _deadline = DateTime.Now;

	[ObservableProperty]
	private string _description = string.Empty;

	[ObservableProperty]
	private ReadOnlyObservableCollection<SubjectDTO> _availableSubjects;
	
	[ObservableProperty]
	private string _linkTitle;
    
	[ObservableProperty]
	private string _linkUrl;

	public CreateTaskViewModel(ITaSkedHomeworks homeworkService, SubjectDataSource subjectSource)
	{
		_homeworkService = homeworkService;
		_subjectSource = subjectSource;

		subjectSource.SubjectSource
			.Connect()
			.ObserveOn(RxApp.MainThreadScheduler)
			.Transform(viewModel => viewModel.SubjectDTO)
			.Bind(out _availableSubjects)
			.Subscribe();

		OnPropertyChanged(nameof(AvailableSubjects));
	}

	[RelayCommand]
	private async Task CreateTask()
	{
		if (string.IsNullOrEmpty(Title) || Subject is null || string.IsNullOrEmpty(LinkUrl))
		{
			return;
		}

		PopUpPage popup = ServiceHelper.GetService<PopUpPage>();
		await popup.IndicateTaskRunningAsync(async () =>
		{
			var request = new CreateHomeworkRequest(Title, Description, Deadline);
			var homework = await _homeworkService.CreateHomework(request, Subject.Id);
			
			if (!string.IsNullOrEmpty(LinkUrl))
			{
				try
				{
					var relatedLink = RelatedLink.Create(new Uri(LinkUrl), LinkTitle);
					var changeRelatedLinkRequest = new ChangeHomeworkRelatedLinksRequest([relatedLink]);
					var updatedHomework = await _homeworkService.ChangeHomeworkSourceUrl(changeRelatedLinkRequest, homework.SubjectId, homework.Id);
					homework.RelatedLinks.AddRange((updatedHomework).RelatedLinks);
				}
				catch (UriFormatException ex)
				{
					await Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "OK");
				}
			}

			TaskViewModel viewModel = new TaskViewModel(homework.CreateTask(), Subject.Name);
			ServiceHelper.GetService<HomeworkDataSource>().HomeworkSource.AddOrUpdate(viewModel);
		});

		await Shell.Current.GoToAsync("..");
	}
}
