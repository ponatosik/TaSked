using LocalizationResourceManager.Maui;
using Refit;
using TaSked.App.Common;
using TaSked.App.Common.Components;

namespace TaSked.App;

public partial class LoginPage : ContentPage
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	private readonly LoginService _loginService;


	public LoginPage(LoginService loginService, ILocalizationResourceManager localizationResourceManager,
		Auth0AuthHandler auth0Method, AnonymousAuthorizationHandler anonymousHandler)
	{
		_loginService = loginService;
		_localizationResourceManager = localizationResourceManager;
		InitializeComponent();
	}
	
	private async void LoginTapped(object sender, EventArgs e)
	{
		try
		{
			var auth0Handler = ServiceHelper.GetService<Auth0AuthHandler>();
			await _loginService.LoginAsync(auth0Handler, new Auth0LoginRequest());
		}
		catch (Exception exception) when (exception is ApiException or AuthenticationException)
		{
			await DisplayAlert("Error", exception.Message, "OK");
		}
		await Shell.Current.GoToAsync("//LoadingPage");
	}

	private async void AnonymousLoginTapped(object sender, EventArgs e)
	{
		string title = _localizationResourceManager["Login_Prompt_Title"];
		string message = _localizationResourceManager["Login_Prompt_Message"];
		string error = _localizationResourceManager["Login_Prompt_Error"];
		string cancel = _localizationResourceManager["General_Alert_OkButton"];
		string? nickname = await DisplayPromptAsync(title, message);

		if (nickname is null)
		{
			return;
		}
		if (string.IsNullOrWhiteSpace(nickname))
		{
			await DisplayAlert(error, message, cancel);
			return;
		}

		try
		{
			var popup = ServiceHelper.GetService<PopUpPage>();
			var anonymousAuthHandler = ServiceHelper.GetService<AnonymousAuthorizationHandler>();
			await popup.IndicateTaskRunningAsync(() =>
				_loginService.LoginAsync(anonymousAuthHandler, new AnonymousLoginRequest(nickname)));
		}
		catch (Exception exception) when (exception is ApiException or AuthenticationException)
		{
			await DisplayAlert("Error", exception.Message, "OK");
		}

		await Shell.Current.GoToAsync("//LoadingPage");
	}
}