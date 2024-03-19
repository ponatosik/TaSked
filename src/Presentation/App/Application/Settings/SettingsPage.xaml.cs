namespace TaSked.App.Application;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	private void JoinRolePage(object sender, EventArgs e)
	{
        Shell.Current.GoToAsync("//RolePage");
    }
}