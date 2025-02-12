using TaSked.Api.ApiClient;

namespace TaSked.App.Common;

internal class UserTokenSecureStorage : IUserTokenStore, IRefreshTokenStore
{
	private const string AccessTokenKey = "TaSked.AccessToken";
	private const string RefreshTokenKey = "TaSked.RefreshToken";
	private const string TokenExpirationKey = "TaSked.AccessTokenExpiration";

	private readonly ISecureStorage _storage;

	private string? _accessToken;
	private string? _refreshToken;
	private string? _accessTokenExpiration;

	public UserTokenSecureStorage(ISecureStorage storage)
	{
		_storage = storage;
	}

	public string? AccessToken
	{
		get => GetFromStorage(AccessTokenKey, ref _accessToken);
		set => _accessToken = SaveTokenToStorage(AccessTokenKey, value);
	}

	public string? RefreshToken
	{
		get => GetFromStorage(RefreshTokenKey, ref _refreshToken);
		set => _refreshToken = SaveTokenToStorage(RefreshTokenKey, value);
	}

	public DateTimeOffset? TokenExpiration
	{
		get => GetFromStorage(TokenExpirationKey, ref _accessTokenExpiration) is not null
			? DateTimeOffset.Parse(_accessTokenExpiration!)
			: null;

		set => _accessTokenExpiration = SaveTokenToStorage(TokenExpirationKey, value?.ToString());
	}

	private string? SaveTokenToStorage(string storageKey, string? value)
	{
		if (value == null)
		{
			_storage.Remove(storageKey);
		}
		else
		{
			_storage.SetAsync(storageKey, value);
		}

		return value;
	}

	private string? GetFromStorage(string storageKey, ref string? token)
	{
		return token ??= _storage.GetAsync(storageKey).Result;
	}
}