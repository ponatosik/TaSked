using Auth0.OidcClient;
using IdentityModel.OidcClient.Browser;
using LocalizationResourceManager.Maui;
using TaSked.Api.ApiClient;

namespace TaSked.App.Common;

public record Auth0LoginRequest : ILoginRequest;

public class Auth0AuthHandler : IAuthHandler<Auth0LoginRequest>
{
	private readonly IUserTokenStore _tokenStore;
	private readonly IRefreshTokenStore _refreshTokenStore;
	private readonly Auth0Client _auth0Client;
	private readonly ILocalizationResourceManager _localizationManager;

	public Auth0AuthHandler(IUserTokenStore tokenStore, IRefreshTokenStore refreshTokenStore,
		Auth0Client auth0Client, ILocalizationResourceManager localizationManager)
	{
		_tokenStore = tokenStore;
		_refreshTokenStore = refreshTokenStore;
		_auth0Client = auth0Client;
		_localizationManager = localizationManager;
	}

	public async Task LoginAsync(Auth0LoginRequest request)
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
		_refreshTokenStore.RefreshToken = loginResult.RefreshToken;
		_refreshTokenStore.TokenExpiration = loginResult.AccessTokenExpiration;
	}

	public Task RestoreSessionAsync()
	{
		return _refreshTokenStore.TokenExpiration < DateTimeOffset.Now ? RefreshToken() : Task.CompletedTask;
	}

	public async Task LogoutAsync()
	{
		var result = await _auth0Client.LogoutAsync();
		if (result != BrowserResultType.Success)
		{
			throw new AuthenticationException("Failed to logout");
		}

		_tokenStore.AccessToken = null;
		_refreshTokenStore.RefreshToken = null;
		_refreshTokenStore.TokenExpiration = null;
	}

	private async Task RefreshToken()
	{
		if (_refreshTokenStore.RefreshToken == null)
		{
			await LogoutAsync();
		}

		var refreshResult = await _auth0Client.RefreshTokenAsync(_refreshTokenStore.RefreshToken);
		if (refreshResult.IsError)
		{
			await LogoutAsync();
			throw new AuthenticationException(refreshResult.Error);
		}

		_tokenStore.AccessToken = refreshResult.AccessToken;
		_refreshTokenStore.RefreshToken = refreshResult.RefreshToken;
		_refreshTokenStore.TokenExpiration = refreshResult.AccessTokenExpiration;
	}
}