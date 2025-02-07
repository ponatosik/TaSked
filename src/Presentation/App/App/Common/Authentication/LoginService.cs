﻿using Akavache;
using Auth0.OidcClient;
using Refit;
using System.Net;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;
using TaSked.App.Common.Notifications;
using TaSked.Domain;

namespace TaSked.App.Common;

public class LoginService
{
	private readonly ITaSkedService _api;
	private readonly ITaSkedUsers _usersService;
	private readonly ITaSkedInvitations _invitationsService;
	private readonly IUserTokenStore _tokenStore;
	private readonly NotificationsService? _notificationsService;
	private readonly IBlobCache? _userCache;
	private readonly Auth0Client _auth0Client;

	public LoginService(
		ITaSkedService api,
		ITaSkedUsers usersService,
		ITaSkedInvitations invitationsService,
		IUserTokenStore tokenStore,
		Auth0Client auth0Client,
		NotificationsService? notificationsService = null,
		IBlobCache? userCache = null)
	{
		_api = api;
		_invitationsService = invitationsService;
		_usersService = usersService;
		_tokenStore = tokenStore;
		_auth0Client = auth0Client;
		_notificationsService = notificationsService;
		_userCache = userCache;
	}

	public async Task CreateGroupAsync(string groupName)
	{
		await _api.CreateGroup(new CreateGroupRequest(groupName));

		if (_notificationsService is not null)
		{
			await _notificationsService.SubscribeToNotifications();
		}
	}
	
	public async Task LeaveGroupAsync()
	{
		if (_notificationsService is not null)
		{
			await _notificationsService.UnsubscribeFromNotifications();
		}
		await _api.LeaveGroup();
	}

	public async Task RegisterAnonymousUser(string username)
	{
		var token = await _api.RegisterAnonymous(new CreateAnonymousUserTokenRequest(username));
		_tokenStore.AccessToken = token;
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
	}

	public async Task JoinGroupAsync(Guid invitation)
	{
		Guid groupId = (await _invitationsService.GetInvitationById(invitation)).GroupId;
		await _invitationsService.ActivateInvitation(new ActivateInvitationRequest(invitation, groupId));

		if (_notificationsService is not null)
		{
			await _notificationsService.SubscribeToNotifications();
		}
	}
	
	public async Task LogoutAsync()
	{
		if (_notificationsService is not null && await HasGroupAsync())
		{
			await _notificationsService.UnsubscribeFromNotifications();
		}
		await _auth0Client.LogoutAsync();
		_tokenStore.AccessToken = null;
	}

	public async Task ClearSessionAsync()
	{
		if (_userCache is not null)
		{
			_userCache.InvalidateAll();
			_userCache.Vacuum();
		}
	}

	public async Task<GroupRole?> GetUserRoleAsync()
	{
		if (!HasAccessToken())
		{
			return null;
		}
		return (await _usersService.GetCurrentUser()).Role;
	}
	
	public async Task<string?> GetUserNicknameAsync()
	{
		if (!HasAccessToken())
		{
			return null;
		}
		return (await _usersService.GetCurrentUser()).Nickname;
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
			user = await _usersService.GetCurrentUser();
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
