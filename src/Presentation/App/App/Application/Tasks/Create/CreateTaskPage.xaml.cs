using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class CreateTaskPage : ContentPage
{

	public CreateTaskPage(CreateTaskViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}