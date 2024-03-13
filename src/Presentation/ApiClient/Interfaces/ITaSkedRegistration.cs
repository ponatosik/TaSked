using Refit;
using TaSked.Api.Requests;

namespace TaSked.Api.ApiClient;

public interface ITaSkedRegistration
{
	[Post("/Users/AnonymousAccout")]
	public Task<string> RegisterAnonymous([Body] CreateUserTokenRequest request);
}
