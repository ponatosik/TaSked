using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateLessonCommand(Guid UserId, Guid SubjectId, DateTime LessonTime) : IRequest<Lesson>;