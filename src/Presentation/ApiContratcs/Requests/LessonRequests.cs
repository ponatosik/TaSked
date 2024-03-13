namespace TaSked.Api.Requests;

public record CreateLessonRequest(Guid SubjectId, DateTime LessonTime);
public record DeleteLessonRequest(Guid SubjectId, Guid LessonId);
public record ChangeLessonTimeRequest(Guid SubjectId, Guid LessonId, DateTime NewTime);
public record GetAllLessonsBySubjectRequest(Guid SubjectId);
public record GetAllLessonsInDateRangeRequest(DateTime StartDate, DateTime EndDate);