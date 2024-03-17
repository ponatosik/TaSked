namespace TaSked.App;

public partial class ReportPage : ContentPage
{
	private RepotrsViewModel _viewModel;

	public ReportPage(RepotrsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		_viewModel.ReloadReports();

		base.OnNavigatedTo(args);
	}
}