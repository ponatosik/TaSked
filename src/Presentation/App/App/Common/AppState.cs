using ReactiveUI;
using TaSked.App.Common;
using TaSked.Domain;

namespace TaSked.App;

public class AppState : ReactiveObject
{
	private readonly IConnectivity _connectivity;
	private readonly LoginService _loginService;

	private bool _hasInternetConnection;
	public bool HasInternetConnection
	{
		get => _hasInternetConnection;
		set => this.RaiseAndSetIfChanged(ref _hasInternetConnection, value);
	}

	private bool _isModerator;
	public bool IsModerator
	{
		get => _isModerator;
		set => this.RaiseAndSetIfChanged(ref _isModerator, value);
	}

	public AppState()
	{
		_connectivity = ServiceHelper.GetService<IConnectivity>();
		_loginService = ServiceHelper.GetService<LoginService>();
		Task.Run(InitializeAsync);
	}

	private async Task InitializeAsync()
	{
		HasInternetConnection = _connectivity.NetworkAccess >= NetworkAccess.ConstrainedInternet;
		GroupRole? role = await _loginService.GetUserRoleAsync();
		IsModerator = role is not null && !(role < GroupRole.Moderator);
		_connectivity.ConnectivityChanged += (obj, args) => HasInternetConnection = args.NetworkAccess >= NetworkAccess.ConstrainedInternet;
	}

}