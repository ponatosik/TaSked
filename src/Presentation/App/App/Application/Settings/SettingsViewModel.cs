using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.App.Common;

namespace TaSked.App.Application;


public partial class SettingsViewModel : ObservableObject
{
	private LoginService _loginService;

	public SettingsViewModel(LoginService loginService)
	{
		_loginService = loginService;
	}

	[RelayCommand]
	public async Task Logout()
	{
		await _loginService.LogoutAsync();
		await _loginService.ClearSessionAsync();
		await Shell.Current.GoToAsync("//LoginPage");
	}
	
	[RelayCommand]
	public async Task LeaveGroup()
	{ 
		await _loginService.LeaveGroupAsync();
		await _loginService.ClearSessionAsync();
		await Shell.Current.GoToAsync("//MainPage");
	}
}
