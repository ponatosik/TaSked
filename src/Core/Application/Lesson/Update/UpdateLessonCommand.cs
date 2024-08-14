using MediatR;
using TaSked.Application.Common;
using TaSked.Domain;

namespace TaSked.Application;

public record UpdateLessonCommand(Guid UserId, Guid SubjectId, Guid LessonId, Optional<DateTime> LessonTime) : IRequest<Lesson>;
