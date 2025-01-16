using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record ChangeSubjectTeacherCommand(Guid UserId, Guid SubjectId, Teacher? NewSubjectTeacher)
	: IRequest<UpdateSubjectDTO>;