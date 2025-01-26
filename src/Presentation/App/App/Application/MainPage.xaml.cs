using Auth0.OidcClient;

namespace TaSked.App;

public partial class MainPage : ContentPage
{
	private readonly Auth0Client auth0Client;
	public MainPage(Auth0Client client)
	{
		InitializeComponent();
		auth0Client = client;
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
	    var loginResult = await auth0Client.LoginAsync(new { audience = "https://tasked.com" });
	    //auth0Client.GetUserInfoAsync();

	    if (!loginResult.IsError)
	    {
		    LoginView.IsVisible = false;
		    HomeView.IsVisible = true;
	    }
	    else
	    {
		    await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
	    }
    }
}
