namespace TaSked.App.Common;

internal class UserTokenSecureStorage : ITokenStore
{
	private const string RefreshTokenKey = "TaSked.RefreshToken";
	private const string AccessTokenKey = "TaSked.AccessToken";
	
	private readonly ISecureStorage _storage;

	private string? _accessToken;
	private string? _refreshToken;

	public UserTokenSecureStorage(ISecureStorage storage)
	{
		_storage = storage;
	}

	public string? AccessToken 
	{
		get => GetTokenFromStorage(AccessTokenKey, ref _accessToken);
		set => SaveTokenToStorage(AccessTokenKey, value);
	}

	public string? RefreshToken
	{
		get => GetTokenFromStorage(RefreshTokenKey, ref _refreshToken);
		set => SaveTokenToStorage(RefreshTokenKey, value);
	}

	private void SaveTokenToStorage(string storageKey, string? token)
	{
		_accessToken = token;
		if (token == null)
		{
			_storage.Remove(storageKey);
		}
		else
		{
			_storage.SetAsync(storageKey, token);
		}
	}

	private string? GetTokenFromStorage(string storageKey, ref string? key)
	{
		return key ??= _storage.GetAsync(storageKey).Result;
	}
}
