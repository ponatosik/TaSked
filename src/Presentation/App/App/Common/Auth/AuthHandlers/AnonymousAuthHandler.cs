using TaSked.Api.ApiClient;
using TaSked.Api.Requests;

namespace TaSked.App.Common;

public record AnonymousLoginRequest(string Username) : ILoginRequest;

public class AnonymousAuthorizationHandler : IAuthHandler<AnonymousLoginRequest>
{
	private readonly IUserTokenStore _tokenStore;
	private readonly ITaSkedService _api;

	public AnonymousAuthorizationHandler(IUserTokenStore tokenStore, ITaSkedService api)
	{
		_tokenStore = tokenStore;
		_api = api;
	}

	public async Task LoginAsync(AnonymousLoginRequest request)
	{
		var userName = request.Username;
		var token = await _api.RegisterAnonymous(new CreateAnonymousUserTokenRequest(userName));
		_tokenStore.AccessToken = token;
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