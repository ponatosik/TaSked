using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.Api.ApiClient;
using TaSked.App.Common;

namespace TaSked.App;

public partial class CreateGroupViewModel : ObservableObject
{
	[ObservableProperty]
	private string _groupName;
	[ObservableProperty]
	private string _userNickname;

	private readonly LoginService _loginService;

    public CreateGroupViewModel(LoginService loginService)
    {
		_loginService = loginService;
	}

	[RelayCommand]
	public async Task CreateGroup()
	{ 
		if (string.IsNullOrEmpty(_groupName) || string.IsNullOrEmpty(_userNickname))
		{
			return;
		}

		await _loginService.CreateGroupAsync(_userNickname, _groupName);

		await Shell.Current.GoToAsync("//UncompletedTasksPage");
	}
}
