namespace TaSked.App;

public partial class TasksPage : ContentPage
{
	public TasksPage(TasksViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}