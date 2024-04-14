﻿using Refit;
using System.Net;
using TaSked.Api.ApiClient;
using TaSked.Domain;

namespace TaSked.App.Common;

public class LoginService
{
	private ITaSkedSevice _api;
	private ITaSkedUsers _usersService;
	private ITaSkedInvitations _invitationsService;
	private IUserTokenStore _tokenStore;

	public LoginService(ITaSkedSevice api, ITaSkedUsers usersService, ITaSkedInvitations invitationsService, IUserTokenStore tokenStore)
	{
		_api = api;
		_invitationsService = invitationsService;
		_usersService = usersService;
		_tokenStore = tokenStore;
	}

	public async Task CreateGroupAsync(string username, string groupName)
	{
		string token = await _api.RegisterAnonymous(new Api.Requests.CreateUserTokenRequest(username));
		_tokenStore.AccessToken = token;
		await _api.CreateGroup(new Api.Requests.CreateGroupRequest(groupName));
	}

	public async Task JoinGroupAsync(string username, Guid invitation)
	{
		string token = await _api.RegisterAnonymous(new Api.Requests.CreateUserTokenRequest(username));
		_tokenStore.AccessToken = token;

		Guid groupId = (await _invitationsService.GetInvitationById(invitation)).GroupId;
		await _invitationsService.ActivateInvitation(new Api.Requests.ActivateInvintationRequest(invitation, groupId));
	}

	public void Logout()
	{
		_tokenStore.AccessToken = null;
		Microsoft.Maui.Controls.Application.Current?.Quit();
	}

	public async Task<GroupRole> GetUserRoleAsync()
	{
		return (await _api.CurrentUser()).Role;
	}

	public async Task<bool> IsAuthorizedAsync()
	{
		return HasAccessToken() && await HasGroup();
	}

	private bool HasAccessToken()
	{
		return _tokenStore.AccessToken != null;
	}

	private async Task<bool> HasGroup()
	{
		User user;

		try
		{
			user = await _usersService.CurrentUser();
		}
		catch (ApiException exception)
		{
			if (exception.StatusCode == HttpStatusCode.Unauthorized ||
				exception.StatusCode == HttpStatusCode.Forbidden)
			{
				return false;
			}
			else
			{
				throw;
			}
		}

		return user.GroupId != null;
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
            user = await _usersService.CurrentUser();
        }
        catch (ApiException exception)
        {
            if (exception.StatusCode == HttpStatusCode.Unauthorized ||
                exception.StatusCode == HttpStatusCode.Forbidden)
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return user.GroupId;
    }
}
