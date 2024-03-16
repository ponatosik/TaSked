using TaSked.Api.ApiClient;

namespace TaSked.App.Common;

public class LoginService
{
	private ITaSkedSevice _api;
	private IUserTokenStore _tokenStore;

	public LoginService(ITaSkedSevice api, IUserTokenStore tokenStore)
	{
		_api = api;
		_tokenStore = tokenStore;

		// Workaround bug.
		// CreateGroupAsync and JoinGroup async would crush app
		// if called wrom viewmodel command
		// and if it is first call to the API.
		// probably bug with refit client initialization
		try
		{
			_api.CurrentUser().Wait();
		}catch (Exception ex) { }
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

		Guid groupId = (await _api.GetInvitationById(invitation)).GroupId;
		await _api.ActivateInvitation(new Api.Requests.ActivateInvintationRequest(invitation,groupId));
	}

	public async Task<bool> IsAuthorized()
	{
		try
		{
			if (_tokenStore.AccessToken == null)
			{
				return false;
			}
			if ((await _api.CurrentUser()).GroupId == null)
			{
				return false;
			}
		} catch (Exception ex)
		{

		}

		return true;
	}
}
