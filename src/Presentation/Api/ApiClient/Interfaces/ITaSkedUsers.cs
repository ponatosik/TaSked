using Refit;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedUsers
{
	[Get("/Users/Account")]
	public Task<User> GetCurrentUser();

	[Post("/Users/{id}")]
	public Task<User> GetUserById(Guid id);
}
