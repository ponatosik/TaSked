using Refit;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedUsers
{
	[Post("/Users/Account")]
	public Task<User> CurrentUser();

	[Post("/Users/{id}")]
	public Task<User> GetUserById(Guid id);
}
