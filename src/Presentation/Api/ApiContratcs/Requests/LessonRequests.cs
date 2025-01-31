using TaSked.Domain;

namespace TaSked.Api.Requests;

public record CreateLessonRequest(Guid SubjectId, DateTime LessonTime, RelatedLink? LessonLink = null);
public record DeleteLessonRequest(Guid SubjectId, Guid LessonId);
public record ChangeLessonTimeRequest(Guid SubjectId, Guid LessonId, DateTime NewTime);

public record ChangeLessonLinkRequest(Guid SubjectId, Guid LessonId, RelatedLink? NewLink);
public record GetAllLessonsBySubjectRequest(Guid SubjectId);
public record GetAllLessonsInDateRangeRequest(DateTime StartDate, DateTime EndDate);