namespace TaSked.App;

public partial class UncompletedTasksPage : ContentPage
{
	public UncompletedTasksPage(UncompletedTasksViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}