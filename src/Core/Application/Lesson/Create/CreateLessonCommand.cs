using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record CreateLessonCommand(Guid UserId, Guid SubjectId, DateTime LessonTime, RelatedLink? LessonUrl = null)
	: IRequest<Lesson>;