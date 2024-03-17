namespace TaSked.App;

public partial class SubjectPage : ContentPage
{
	private SubjectsViewModel _viewModel;

	public SubjectPage(SubjectsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		_viewModel.ReloadSubjects();

		base.OnNavigatedTo(args);
	}
}