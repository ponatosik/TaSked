using Auth0.OidcClient;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using LocalizationResourceManager.Maui;
using System.Text.RegularExpressions;
using TaSked.Api.ApiClient;
using Regex = System.Text.RegularExpressions.Regex;

namespace TaSked.App.Common;

public record Auth0LoginRequest : ILoginRequest;

public partial class Auth0AuthHandler : IAuthHandler<Auth0LoginRequest>
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

		AssertLoginResult(loginResult);

		if (loginResult.AccessToken == null)
		{
			throw new AuthenticationException(_localizationManager["Login.Authorization.Error.General"]);
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


	private void AssertLoginResult(LoginResult loginResult)
	{
		if (!loginResult.IsError)
		{
			return;
		}

		if (loginResult.Error != "access_denied")
		{
			throw new AuthenticationException(loginResult.Error);
		}

		if (!UserNicknameTakenRegex().IsMatch(loginResult.ErrorDescription))
		{
			throw new AuthenticationException(_localizationManager["Login_Authorization_Error_General"]);
		}

		var userName = UserNicknameTakenRegex().Match(loginResult.ErrorDescription).Groups[1].Value;
		var errorMessage = string.Format(_localizationManager["Login_Authorization_Error_NicknameTaken"], userName);
		throw new AuthenticationException(errorMessage);
	}

	[GeneratedRegex("Nickname (.+?) is already taken")]
	private static partial Regex UserNicknameTakenRegex();
}