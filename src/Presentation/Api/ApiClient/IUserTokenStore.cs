namespace TaSked.Api.ApiClient;

public interface IUserTokenStore
{
	public string? AccessToken { get; set; }
}
