using TaSked.Application;
using TaSked.Domain;
using Refit;

namespace TaSked.Api.ApiClient;

public interface ITaskedApiClient
{
	[Get("/homework")]
	public Task<IEnumerable<HomeworkTask>> GetAllTasks();


	[Post("/homework")]
	public Task CreateTask(CreateHomeworkCommand command);

}
