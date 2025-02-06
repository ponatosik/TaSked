using TaSked.Domain;

namespace TaSked.Api.Requests;

public record CreateLessonRequest(DateTime LessonTime, RelatedLink? LessonLink = null);

public record ChangeLessonTimeRequest(DateTime NewTime);

public record ChangeLessonLinkRequest(RelatedLink? NewLink);
public record GetAllLessonsBySubjectRequest(Guid SubjectId);
public record GetAllLessonsInDateRangeRequest(DateTime StartDate, DateTime EndDate);