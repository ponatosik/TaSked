namespace TaSked.App.Application;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}

	private async void JoinRolePage(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync("//RolePage");
    }
}