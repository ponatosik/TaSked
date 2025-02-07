using Refit;
using TaSked.Api.Requests;

namespace TaSked.Api.ApiClient;

public interface ITaSkedRegistration
{
	[Post("/Users/Anonymous")]
	public Task<string> RegisterAnonymous([Body] CreateAnonymousUserTokenRequest request);
}
