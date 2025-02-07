using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record ChangeLessonLinkCommand(Guid UserId, Guid SubjectId, Guid LessonId, RelatedLink? LessonLink)
	: IRequest<Lesson>;