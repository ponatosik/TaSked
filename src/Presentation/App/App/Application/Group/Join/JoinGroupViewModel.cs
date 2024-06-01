using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;
using TaSked.App.Common;

namespace TaSked.App;

public partial class JoinGroupViewModel : ObservableObject
{
	private readonly LoginService _loginService;

	[ObservableProperty]
	private string _groupInvitationId;

	[ObservableProperty]
	private string _userNickname;

	[ObservableProperty]
	private IReactiveCommand _joinGroupCommand;


    public JoinGroupViewModel(LoginService loginService)
    {
		_loginService = loginService;
		JoinGroupCommand = ReactiveCommand.CreateFromTask(JoinGroup);
    }

	public async Task JoinGroup()
	{ 
		if (string.IsNullOrEmpty(_groupInvitationId) 
			|| string.IsNullOrEmpty(_userNickname)
			|| !Guid.TryParse(_groupInvitationId, out Guid invitationId))
		{
			return;
		}

		await _loginService.JoinGroupAsync(_userNickname, invitationId);
        
        await Shell.Current.GoToAsync("//UncompletedTasksPage");
    }
}
