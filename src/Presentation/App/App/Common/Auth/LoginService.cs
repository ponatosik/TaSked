using Akavache;
using Refit;
using System.Net;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Common;

public class LoginService
{
	private readonly ITaSkedService _api;
	private readonly IUserTokenStore _tokenStore;
	private readonly IBlobCache? _userCache;
	private readonly IAuthHandlerManager _authHandlerManager;
	
	public event Action? LoginCompleted;
	public event Action? LogoutCompleted;
	public event Action? GroupJoined;
	public event Action? GroupLeft;

	public LoginService(ITaSkedService api, IUserTokenStore tokenStore, IAuthHandlerManager authHandlerManager,
		IBlobCache? userCache = null)
	{
		_api = api;
		_tokenStore = tokenStore;
		_authHandlerManager = authHandlerManager;
		_userCache = userCache;
	}

	public async Task RestoreSessionAsync()
	{
		var authorizationMethod = await _authHandlerManager.LoadCurrentAuthHandlerAsync();
		if (authorizationMethod != null)
		{
			await authorizationMethod.RestoreSessionAsync();
		}
	}

	public async Task CreateGroupAsync(string groupName)
	{
		await _api.CreateGroup(new CreateGroupRequest(groupName));

		_userCache?.InvalidateAllObjects<User>();
		_userCache?.Vacuum();
		
		GroupJoined?.Invoke();
	}
	
	public async Task LeaveGroupAsync()
	{
		await _api.LeaveGroup();
		await ClearSessionAsync();
		GroupLeft?.Invoke();
	}


	public async Task LoginAsync<TLoginMethod, TLoginRequest>(
		TLoginMethod authorizationMethod,
		TLoginRequest request
	)
		where TLoginRequest : ILoginRequest
		where TLoginMethod : IAuthHandler<TLoginRequest>
	{
		await authorizationMethod.LoginAsync(request);
		await _authHandlerManager.SaveCurrentAuthHandlerAsync(authorizationMethod);
		LoginCompleted?.Invoke();
	}

	public async Task JoinGroupAsync(Guid invitation)
	{
		var groupId = (await _api.GetInvitationById(invitation)).GroupId;
		await _api.ActivateInvitation(new ActivateInvitationRequest(invitation, groupId));
		GroupJoined?.Invoke();
	}
	
	public async Task LogoutAsync()
	{
		var authorizationMethod = await _authHandlerManager.LoadCurrentAuthHandlerAsync();
		if (authorizationMethod == null)
		{
			return;
		}

		await authorizationMethod.LogoutAsync();
		await ClearSessionAsync();
		await _authHandlerManager.RemoveCurrentAuthHandlerAsync();
		
		LogoutCompleted?.Invoke();
	}

	public Task ClearSessionAsync()
	{
		if (_userCache is null)
		{
			return Task.CompletedTask;
		}

		_userCache.InvalidateAll();
		_userCache.Vacuum();

		return Task.CompletedTask;
	}

	public async Task<GroupRole?> GetUserRoleAsync()
	{
		return (await GetUserAsync())?.Role;
	}


	public async Task<string?> GetUserNicknameAsync()
	{
		return (await GetUserAsync())?.Nickname;
	}

	public Task<bool> HasGroupAsync()
	{
		return GetGroupIdAsync().ContinueWith(groupId => groupId.Result is not null);
	}

	public async Task<Guid?> GetGroupIdAsync()
	{
		var user = await GetUserAsync();
		return user?.GroupId;
	}

	private bool HasAccessToken()
	{
		return _tokenStore.AccessToken != null;
	}


	private async Task<User?> GetUserAsync()
	{
		if (!HasAccessToken())
		{
			return null;
		}

		try
		{
			return await _api.GetCurrentUser();
		}
		catch (ApiException exception)
		{
			if (exception.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
			{
				return null;
			}

			throw;
		}
	}
}
