using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using TaSked.App.Common;

namespace TaSked.App;

public partial class CreateGroupViewModel : ObservableObject
{
	[ObservableProperty]
	private string _groupName;
	[ObservableProperty]
	private string _userNickname;
	[ObservableProperty]
	private IReactiveCommand _createGroupCommand;

	private readonly LoginService _loginService;

    public CreateGroupViewModel(LoginService loginService)
    {
		_loginService = loginService;

		CreateGroupCommand = ReactiveCommand.CreateFromTask(CreateGroup);
	}

	public async Task CreateGroup()
	{
		if (string.IsNullOrEmpty(GroupName) || string.IsNullOrEmpty(UserNickname))
		{
			return;
		}

		await _loginService.RegisterAnonymousUser(UserNickname);
		await _loginService.CreateGroupAsync(GroupName);

		await Shell.Current.GoToAsync("//UncompletedTasksPage");
	}
}
