using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedHomeworks
{

	[Get("/Homeworks")]
	public Task<List<Homework>> GetAllHomework();

	[Post("/Homeworks")]
	public Task<Homework> CreateHomework([Body] CreateHomeworkRequest request);

	[Delete("/Homeworks")]
	public Task DeleteHomework([Body] DeleteHomeworkRequest request);

	[Patch("/Homeworks/Deadline")]
	public Task<Homework> ChangeDeadline([Body] ChangeHomeworkDeadlineRequest request);

	[Patch("/Homeworks/Description")]
	public Task<Homework> ChangeDescription([Body] ChangeHomeworkDescriptionRequest request);

	[Patch("/Homeworks/RelatedLinks")]
	public Task<Homework> ChangeSourceUrl([Body] ChangeHomeworkSourceUrlRequest request);

	[Patch("/Homeworks/Title")]
	public Task<Homework> ChangeTitle([Body] ChangeHomeworkTitleRequest request);
}
