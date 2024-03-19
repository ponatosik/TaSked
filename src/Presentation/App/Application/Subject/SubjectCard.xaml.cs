using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.App.Components;

public partial class SubjectCard: ContentView
{
	public static readonly BindableProperty SubjectDTOModelProperty =
		BindableProperty.Create(nameof(SubjectDTOModel), typeof(SubjectDTO), typeof(SubjectCard));

	public SubjectDTO SubjectDTOModel
	{
		get => (SubjectDTO)GetValue(SubjectDTOModelProperty);
		set => SetValue(SubjectDTOModelProperty, value);
	}

	public SubjectCard()
	{
		InitializeComponent();
	}

	private async void Update_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("UpdateSubjectPage", new Dictionary<string, object>
		{
			["subject"] = SubjectDTOModel
		});	
	}

	private async void Delete_Clicked(object sender, EventArgs e)
	{
		ITaSkedSevice api = ServiceHelper.GetService<ITaSkedSevice>();
		SubjectsViewModel viewModel = ServiceHelper.GetService<SubjectsViewModel>();

		var request = new DeleteSubjectRequest(SubjectDTOModel.Id);
		await api.DeleteSubject(request);

		viewModel.Subjects.Remove(SubjectDTOModel);
	}
}