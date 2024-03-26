namespace TaSked.App;

public partial class SubjectPage : ContentPage
{
	private SubjectsViewModel _viewModel;

	public SubjectPage(SubjectsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		_viewModel.ReloadSubjects();
		BindingContext = _viewModel;
	}
}