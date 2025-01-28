using Refit;
using TaSked.App.Common;

namespace TaSked.App;

public partial class MainPage : ContentPage
{
	private readonly LoginService _loginService;

	public MainPage(LoginService loginService)
	{
		_loginService = loginService;
		InitializeComponent();
	}

    private void JoinGroupTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("JoinGroupPage");
    }

    private void CreateGroupTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("CreateGroupPage");
    }
    
    private async void OnLoginClicked(object sender, EventArgs e)
    {
	    try
	    {
		    await _loginService.LoginWithAuth0();
	    }
	    catch (Exception exception) when (exception is ApiException or AuthenticationException)
	    {
		    await DisplayAlert("Error", exception.Message, "OK");
	    }
    }
}
