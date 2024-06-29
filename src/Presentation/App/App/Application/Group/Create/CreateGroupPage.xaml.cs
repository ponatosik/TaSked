namespace TaSked.App;

public partial class CreateGroupPage : ContentPage
{
	public CreateGroupPage(CreateGroupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}