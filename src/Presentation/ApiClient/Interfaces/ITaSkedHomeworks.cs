using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedHomeworks
{

	[Get("/Homeworks")]
	public Task<List<Homework>> GetAllHomework();

	[Post("/Homeworks")]
	public Task<Homework> CreateHomework(CreateHomeworkRequest request);

	[Delete("/Homeworks")]
	public Task DeleteHomework();

	[Patch("/Homeworks/Deadline")]
	public Task<Homework> ChangeDeadline(ChangeHomeworkDeadlineRequest request);

	[Patch("/Homeworks/Description")]
	public Task<Homework> ChangeDescription(ChangeHomeworkDescriptionRequest request);

	[Patch("/Homeworks/SourceUrl")]
	public Task<Homework> ChangeSourceUrl(ChangeHomeworkSourceUrlRequest request);

	[Patch("/Homeworks/Title")]
	public Task<Homework> ChangeTitle(ChangeHomeworkTitleRequest request);
}
