using TaSked.Api.ApiClient;

namespace TaSked.App.Common;

internal class UserTokenSecureStorage : IUserTokenStore
{
	private const string SECURE_STORAGE_KEY = "TaSked.AccessToken";
	private readonly ISecureStorage _storage;

	private string? _accessToken;

	public UserTokenSecureStorage(ISecureStorage storage)
	{
		_storage = storage;
	}

	public string? AccessToken 
	{
		get
		{
			if(_accessToken == null)
			{
				_accessToken = _storage.GetAsync(SECURE_STORAGE_KEY).Result;
			}
			return _accessToken;
		}
		set 
		{
			_accessToken = value;
			_storage.SetAsync(SECURE_STORAGE_KEY, value);
		}
	}
}
