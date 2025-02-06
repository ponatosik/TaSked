using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.App.Common;

namespace TaSked.App.Application;


public partial class MainPageViewModel : ObservableObject
{
	private readonly LoginService _loginService;
	
	[ObservableProperty]
	private string _nickname;

	public MainPageViewModel(LoginService loginService)
	{
		_loginService = loginService;
	}
	
	public async Task LoadUserNicknameAsync()
	{
		Nickname = await _loginService.GetUserNicknameAsync();
	}

	[RelayCommand]
	public async Task Logout()
	{
		await _loginService.LogoutAsync();
		await _loginService.ClearSessionAsync();
		await Shell.Current.GoToAsync("//LoginPage");
	}
}