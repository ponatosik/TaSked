using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class UpdateSubjectPage : ContentPage
{

	public UpdateSubjectPage(UpdateSubjectViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}