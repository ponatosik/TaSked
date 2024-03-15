using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaSked.App.Common;

namespace TaSked.App;

public partial class JoinGroupViewModel : ObservableObject
{
	private readonly LoginService _loginService;

	[ObservableProperty]
	private string _groupInvitationId;
	[ObservableProperty]
	private string _userNickname;

    public JoinGroupViewModel(LoginService loginService)
    {
		_loginService = loginService;
    }

	[RelayCommand]
	public async Task JoinGroup()
	{ 
		if (string.IsNullOrEmpty(_groupInvitationId) 
			|| string.IsNullOrEmpty(_userNickname)
			|| !Guid.TryParse(_groupInvitationId, out Guid invitationId))
		{
			return;
		}

		await _loginService.JoinGroupAsync(_userNickname, invitationId);
        
        await Shell.Current.GoToAsync("//TasksPage");
    }
}
