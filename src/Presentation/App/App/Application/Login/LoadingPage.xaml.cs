using TaSked.App.Common;
using TaSked.Domain;

namespace TaSked.App;

public partial class LoadingPage : ContentPage
{
    private readonly LoginService _loginService;
    public LoadingPage(LoginService loginService)
	{
		InitializeComponent();
        _loginService = loginService;
        Loaded += async (_, _) => await LoadUser();
	}

	private async Task LoadUser()
    {
	    await _loginService.RestoreSessionAsync();

		if (await _loginService.HasGroupAsync())
        {
            await Shell.Current.GoToAsync("//UncompletedTasksPage");
        }
        else if ((await _loginService.GetUserRoleAsync()) == GroupRole.NoGroup )
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
	        await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}