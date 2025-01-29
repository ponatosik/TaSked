using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Refit;
using TaSked.App.Common;

namespace TaSked.App;

public partial class LoginPage : ContentPage
{
	private readonly LoginService _loginService;
	
	public LoginPage(LoginService loginService)
	{
		_loginService = loginService;
		InitializeComponent();
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
		await Shell.Current.GoToAsync("//LoadingPage");
	}
	
	private async void OnLoginClicked2(object sender, EventArgs e)
	{
		try
		{
			await _loginService.RegisterAnonymousUser("user");
		}
		catch (Exception exception) when (exception is ApiException or AuthenticationException)
		{
			await DisplayAlert("Error", exception.Message, "OK");
		}
		await Shell.Current.GoToAsync("//LoadingPage");
	}
}