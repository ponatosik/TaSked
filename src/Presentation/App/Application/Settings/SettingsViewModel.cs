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
	public void Logout() 
	{
		_loginService.Logout();
	}
}
