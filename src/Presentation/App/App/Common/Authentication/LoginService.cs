using Akavache;
using Auth0.OidcClient;
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
	private readonly Auth0Client _auth0Client;

	public event Action? LoginCompleted;
	public event Action? LogoutCompleted;
	public event Action? GroupJoined;
	public event Action? GroupLeft;

	public LoginService(
		ITaSkedService api,
		IUserTokenStore tokenStore,
		Auth0Client auth0Client,
		IBlobCache? userCache = null)
	{
		_api = api;
		_tokenStore = tokenStore;
		_auth0Client = auth0Client;
		_userCache = userCache;
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

		_userCache?.InvalidateAll();
		_userCache?.Vacuum();
		GroupLeft?.Invoke();
	}

	public async Task RegisterAnonymousUser(string username)
	{
		var token = await _api.RegisterAnonymous(new CreateAnonymousUserTokenRequest(username));
		_tokenStore.AccessToken = token;
		LoginCompleted?.Invoke();
	}

	public async Task LoginWithAuth0()
	{
		var loginResult = await _auth0Client.LoginAsync(new { audience = "https://tasked.com" });

		if (loginResult.IsError)
		{
			throw new AuthenticationException(loginResult.Error);
		}

		if (loginResult.AccessToken == null)
		{
			throw new AuthenticationException("Failed to login");
		}

		_tokenStore.AccessToken = loginResult.AccessToken;
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
		await _auth0Client.LogoutAsync();
		_tokenStore.AccessToken = null;

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
		return !HasAccessToken() ? null : (await _api.GetCurrentUser()).Role;
	}
	
	public async Task<string?> GetUserNicknameAsync()
	{
		return !HasAccessToken() ? null : (await _api.GetCurrentUser()).Nickname;
	}

	public Task<bool> HasGroupAsync()
	{
		return GetGroupIdAsync().ContinueWith(groupId => groupId.Result is not null);
	}

	private bool HasAccessToken()
	{
		return _tokenStore.AccessToken != null;
	}

	public async Task<Guid?> GetGroupIdAsync()
	{
		if (!HasAccessToken())
		{
			return null;
		}

		User user;

		try
		{
			user = await _api.GetCurrentUser();
		}
		catch (ApiException exception)
		{
			if (exception.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
			{
				return null;
			}

			throw;
		}

		return user.GroupId;
	}
}
