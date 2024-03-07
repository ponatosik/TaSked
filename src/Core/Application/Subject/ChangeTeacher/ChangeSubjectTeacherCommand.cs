using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeSubjectTeacherCommand(Guid UserId, Guid SubjectId, Teacher NewSubjectTeacher) : IRequest<Subject>;