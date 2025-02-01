using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using LocalizationResourceManager.Maui;
using TaSked.App.Common;

namespace TaSked.App;

public partial class LoginPage : ContentPage
{
	private readonly ILocalizationResourceManager _localizationResourceManager;
	
	private readonly LoginService _loginService;
	
	
	public LoginPage(LoginService loginService, ILocalizationResourceManager localizationResourceManager)
	{
		_loginService = loginService;
		_localizationResourceManager = localizationResourceManager;
		InitializeComponent();
	}
	
	private async void LoginTapped(object sender, EventArgs e)
	{
		try
		{
			await _loginService.LoginWithAuth0();
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
		}
		else
		{
			try
			{
				await _loginService.RegisterAnonymousUser(nickname);
			}
			catch (Exception exception) when (exception is ApiException or AuthenticationException)
			{
				await DisplayAlert("Error", exception.Message, "OK");
			}
			await Shell.Current.GoToAsync("//LoadingPage");
		}
	}
}