using TaSked.App.Common.Models;
using TaSked.Domain;

namespace TaSked.App;

public partial class UpdateSubjectPage : ContentPage
{
	private readonly UpdateSubjectViewModel _viewModel;

	public UpdateSubjectPage(UpdateSubjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		_viewModel = viewModel;
	}


	protected override void OnAppearing()
	{
		_viewModel.Teachers = [.._viewModel.SubjectDTO.Teachers.Select(UpdateTeacherDTO.From)];
		_viewModel.RelatedLinkInputs = [.._viewModel.SubjectDTO.RelatedLinks.Select(link => new RelatedLinkModel(link.Title, link.Url.ToString()))];
		base.OnAppearing();
	}
}