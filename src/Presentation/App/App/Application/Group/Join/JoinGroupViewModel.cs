using CommunityToolkit.Mvvm.ComponentModel;
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
		if (string.IsNullOrEmpty(GroupInvitationId)
		    || string.IsNullOrEmpty(UserNickname)
		    || !Guid.TryParse(GroupInvitationId, out var invitationId))
		{
			return;
		}

		await _loginService.RegisterAnonymousUser(UserNickname);
		await _loginService.JoinGroupAsync(invitationId);
        
        await Shell.Current.GoToAsync("//UncompletedTasksPage");
    }
}
