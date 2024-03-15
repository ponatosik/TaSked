namespace TaSked.App;

public partial class JoinGroupPage : ContentPage
{
	public JoinGroupPage(JoinGroupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void JoinClick(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//TasksPage");
    }
}