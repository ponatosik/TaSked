using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class SubjectDetailsPage : ContentPage
{

	public SubjectDetailsPage(SubjectDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}