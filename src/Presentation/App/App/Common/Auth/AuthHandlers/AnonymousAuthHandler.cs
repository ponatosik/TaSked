using LocalizationResourceManager.Maui;
using Refit;
using System.Net;
using TaSked.Api.ApiClient;
using TaSked.Api.Requests;

namespace TaSked.App.Common;

public record AnonymousLoginRequest(string Username) : ILoginRequest;

public class AnonymousAuthorizationHandler : IAuthHandler<AnonymousLoginRequest>
{
	private readonly IUserTokenStore _tokenStore;
	private readonly ITaSkedService _api;
	private readonly ILocalizationResourceManager _localizationManager;

	public AnonymousAuthorizationHandler(IUserTokenStore tokenStore, ITaSkedService api,
		ILocalizationResourceManager localizationManager)
	{
		_tokenStore = tokenStore;
		_api = api;
		_localizationManager = localizationManager;
	}

	public async Task LoginAsync(AnonymousLoginRequest request)
	{
		var userName = request.Username;

		try
		{
			var token = await _api.RegisterAnonymous(new CreateAnonymousUserTokenRequest(userName));
			_tokenStore.AccessToken = token;
		}
		catch (ApiException exception) when (exception.StatusCode is HttpStatusCode.Conflict)
		{
			var errorMessage = string.Format(_localizationManager["Login_Authorization_Error_NicknameTaken"], userName);
			throw new AuthenticationException(errorMessage);
		}
	}

	public Task RestoreSessionAsync()
	{
		return Task.CompletedTask;
	}

	public Task LogoutAsync()
	{
		_tokenStore.AccessToken = null;
		return Task.CompletedTask;
	}
}