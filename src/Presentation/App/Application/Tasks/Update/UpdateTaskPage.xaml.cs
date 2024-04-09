using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class UpdateTaskPage : ContentPage
{
	public UpdateTaskPage(UpdateTaskViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}