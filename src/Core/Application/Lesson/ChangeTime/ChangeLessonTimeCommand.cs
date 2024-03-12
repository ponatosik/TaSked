using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeLessonTimeCommand(Guid UserId, Guid SubjectId, Guid LessonId, DateTime NewTime) : IRequest<Lesson>;