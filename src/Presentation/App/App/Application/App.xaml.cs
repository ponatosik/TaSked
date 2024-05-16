namespace TaSked.App;

public partial class App : Microsoft.Maui.Controls.Application
{
	public App()
	{
		InitializeComponent();
        Microsoft.Maui.Controls.Application.Current.UserAppTheme = AppTheme.Dark;
		MainPage = new AppShell();
	}
}
