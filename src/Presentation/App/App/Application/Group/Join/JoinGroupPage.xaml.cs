namespace TaSked.App;

public partial class JoinGroupPage : ContentPage
{
	public JoinGroupPage(JoinGroupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}