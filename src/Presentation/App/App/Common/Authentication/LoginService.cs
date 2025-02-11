using Akavache;
using Auth0.OidcClient;
using LocalizationResourceManager.Maui;
using Refit;
using System.Net;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.App.Common;

public class LoginService
{
	private readonly ITaSkedService _api;
	private readonly ITokenStore _tokenStore;
	private readonly IBlobCache? _userCache;
	private readonly Auth0Client _auth0Client;
	private readonly ILocalizationResourceManager _localizationManager;

	public event Action? LoginCompleted;
	public event Action? LogoutCompleted;
	public event Action? GroupJoined;
	public event Action? GroupLeft;

	public LoginService(
		ITaSkedService api,
		ITokenStore tokenStore,
		Auth0Client auth0Client,
		ILocalizationResourceManager localizationManager,
		IBlobCache? userCache = null)
	{
		_api = api;
		_tokenStore = tokenStore;
		_auth0Client = auth0Client;
		_localizationManager = localizationManager;
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
		var loginResult = await _auth0Client.LoginAsync(new
		{
			audience = "https://tasked.com", ui_locales = _localizationManager.CurrentCulture.Name
		});

		if (loginResult.IsError)
		{
			throw new AuthenticationException(loginResult.Error);
		}

		if (loginResult.AccessToken == null)
		{
			throw new AuthenticationException("Failed to login");
		}

		_tokenStore.AccessToken = loginResult.AccessToken;
		_tokenStore.RefreshToken = loginResult.RefreshToken;
		
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
		catch (ApiException apiException)
		{
			if (apiException.StatusCode != HttpStatusCode.Unauthorized || _tokenStore.RefreshToken is null)
			{
				throw;
			}

			var refreshResult = await _auth0Client.RefreshTokenAsync(_tokenStore.RefreshToken);
			if (refreshResult.IsError)
			{
				throw new AuthenticationException(refreshResult.Error);
			}

			_tokenStore.AccessToken = refreshResult.AccessToken;
			_tokenStore.RefreshToken = refreshResult.RefreshToken;

			return await _api.GetCurrentUser();
		}
	}
}
