namespace TaSked.App;

public partial class CreateGroupPage : ContentPage
{
	public CreateGroupPage(CreateGroupViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void CreateClick(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//TasksPage");
    }
}