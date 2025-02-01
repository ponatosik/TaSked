using TaSked.Api.ApiClient;

namespace TaSked.App.Common;

internal class UserTokenSecureStorage : IUserTokenStore
{
	private const string SecureStorageKey = "TaSked.AccessToken";
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
				_accessToken = _storage.GetAsync(SecureStorageKey).Result;
			}
			return _accessToken;
		}
		set 
		{
			_accessToken = value;
			if( value == null ) 
			{
				_storage.Remove(SecureStorageKey);
			}
			else
			{
				_storage.SetAsync(SecureStorageKey, value);
			}
		}
	}
}
