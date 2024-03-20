using TaSked.App.Common;

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
		RolePage rolePage = ServiceHelper.GetService<RolePage>();
		await Navigation.PushAsync(rolePage);
    }
}