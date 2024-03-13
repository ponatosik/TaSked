using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedLessons
{
	[Post("/Lessons/")]
	public Task<Lesson> Create([Body] CreateLessonRequest request);

	[Delete("/Lessons/")]
	public Task Delete([Body] DeleteLessonRequest request);

	[Patch("/Lessons/Time")]
	public Task<Lesson> ChangeTime([Body] ChangeLessonTimeRequest request);

	[Get("/Lessons/BySubject/{SubjectId}")]
	public Task<List<Lesson>> GetBySubject(Guid SubjectId);

	[Get("/Lessons/ByDateRange")]
	public Task<List<Lesson>> Get([Query("fromDate")] DateTime? from = null, [Query("toDate")] DateTime? to = null);
}
