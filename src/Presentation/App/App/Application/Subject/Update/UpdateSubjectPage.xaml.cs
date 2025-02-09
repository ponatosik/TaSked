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
		base.OnAppearing();
	}
}