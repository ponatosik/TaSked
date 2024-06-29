using TaSked.Api.ApiClient;

namespace TaSked.App;

public partial class CreateReportPage : ContentPage
{

	public CreateReportPage(CreateReportViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}