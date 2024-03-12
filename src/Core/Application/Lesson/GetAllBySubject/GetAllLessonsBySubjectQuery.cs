using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetAllLessonsBySubjectQuery(Guid UserId, Guid SubjectId) : IRequest<List<Lesson>>;