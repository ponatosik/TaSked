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
		IsModerator = await CheckIsModerator();

		_loginService.LoginCompleted += async () => IsModerator = await CheckIsModerator();
		_loginService.GroupJoined += async () => IsModerator = await CheckIsModerator();
		_loginService.GroupLeft += () => IsModerator = false;
		_connectivity.ConnectivityChanged += (obj, args) => HasInternetConnection = args.NetworkAccess >= NetworkAccess.ConstrainedInternet;
	}

	private async Task<bool> CheckIsModerator()
	{
		var role = await _loginService.GetUserRoleAsync();
		return role != null && !(role < GroupRole.Moderator);
	}
}