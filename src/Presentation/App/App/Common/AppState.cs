using CommunityToolkit.Mvvm.ComponentModel;
using TaSked.Domain;

namespace TaSked.App.Common;

public partial class AppState : ObservableObject
{
	private readonly IConnectivity _connectivity;
	private readonly LoginService _loginService;

	[ObservableProperty]
	private bool _isBusy = true;

	[ObservableProperty]
	private bool _hasInternet = false;

	[ObservableProperty]
	private bool _isModerator = false;

	public AppState(IConnectivity connectivity, LoginService loginService)
	{
		_connectivity = connectivity;
		_loginService = loginService;
		Task.Run(InitializeAsync);
	}

	private async Task InitializeAsync()
	{
		HasInternet = _connectivity.NetworkAccess == NetworkAccess.Internet;
		IsModerator = !(await _loginService.GetUserRoleAsync() < GroupRole.Moderator);
		IsBusy = false;
	}
}