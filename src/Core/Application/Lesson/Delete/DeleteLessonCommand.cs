using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record DeleteLessonCommand(Guid UserId, Guid SubjectId, Guid LessonId) : IRequest;