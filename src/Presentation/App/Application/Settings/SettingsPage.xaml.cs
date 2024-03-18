namespace TaSked.App.Application;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}