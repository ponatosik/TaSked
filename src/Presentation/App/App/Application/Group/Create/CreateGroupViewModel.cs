using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
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
		if (string.IsNullOrEmpty(GroupName))
		{
			return;
		}
		await _loginService.CreateGroupAsync(GroupName);

		await Shell.Current.GoToAsync("//UncompletedTasksPage");
	}
}
