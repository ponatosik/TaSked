using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedLessons
{
	[Post("/Subjects/{subjectId}/Lessons")]
	public Task<Lesson> CreateLesson([Body] CreateLessonRequest request, Guid subjectId);

	[Delete("/Subjects/{subjectId}/Lessons/{lessonId}")]
	public Task DeleteLesson(Guid subjectId, Guid lessonId);

	[Patch("/Subjects/{subjectId}/Lessons/{lessonId}/Time")]
	public Task<Lesson> ChangeLessonTime([Body] ChangeLessonTimeRequest request, Guid subjectId, Guid lessonId);

	[Patch("/Subjects/{subjectId}/Lessons/{lessonId}/Link")]
	public Task<Lesson> ChangeLessonLink([Body] ChangeLessonLinkRequest request, Guid subjectId, Guid lessonId);

	[Get("/Subjects/{subjectId}/Lessons")]
	public Task<List<Lesson>> GetSubjectLessons(Guid subjectId);

	[Get("/Lessons")]
	public Task<List<Lesson>> Get([Query("fromDate")] DateTime? from = null, [Query("toDate")] DateTime? to = null);
}
