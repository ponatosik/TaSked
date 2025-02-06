using Refit;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedHomeworks
{
	[Get("/Homework")]
	public Task<List<Homework>> GetAllHomework();

	[Post("/Subjects/{subjectId}/Homework")]
	public Task<Homework> CreateHomework([Body] CreateHomeworkRequest request, Guid subjectId);

	[Delete("/Subjects/{subjectId}/Homework/{homeworkId}")]
	public Task DeleteHomework(Guid subjectId, Guid homeworkId);

	[Patch("/Subjects/{subjectId}/Homework/{homeworkId}/Deadline")]
	public Task<Homework> ChangeHomeworkDeadline([Body] ChangeHomeworkDeadlineRequest request, Guid subjectId,
		Guid homeworkId);

	[Patch("/Subjects/{subjectId}/Homework/{homeworkId}/Description")]
	public Task<Homework> ChangeHomeworkDescription([Body] ChangeHomeworkDescriptionRequest request, Guid subjectId,
		Guid homeworkId);

	[Patch("/Subjects/{subjectId}/Homework/{homeworkId}/RelatedLinks")]
	public Task<Homework> ChangeHomeworkSourceUrl([Body] ChangeHomeworkRelatedLinksRequest request, Guid subjectId,
		Guid homeworkId);

	[Patch("/Subjects/{subjectId}/Homework/{homeworkId}/Title")]
	public Task<Homework> ChangeHomeworkTitle([Body] ChangeHomeworkTitleRequest request, Guid subjectId,
		Guid homeworkId);

	[Patch("/Subjects/{subjectId}/Homework/{homeworkId}/BriefSummary")]
	public Task<Homework> ChangeHomeworkBriefSummary([Body] ChangeHomeworkBriefSummaryRequest request, Guid subjectId,
		Guid homeworkId);

	[Get("/Subjects/{subjectId}/Homework/{homeworkId}/Comments")]
	public Task<List<CommentDTO>> GetHomeworkComments(Guid subjectId, Guid homeworkId);

	[Post("/Subjects/{subjectId}/Homework/{homeworkId}/Comments")]
	public Task<CommentDTO> CommentHomework([Body] CommentHomeworkRequest request, Guid subjectId, Guid homeworkId);
}
