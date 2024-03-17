using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class CreateSubjectPage : ContentPage
{

	public CreateSubjectPage(CreateSubjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}