namespace TaSked.App;

public partial class CreateAnnouncementPage : ContentPage
{
	public CreateAnnouncementPage(CreateAnnouncementViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}