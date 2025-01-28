using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class TasksDetailsPage : ContentPage
{

	public TasksDetailsPage(TasksDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}