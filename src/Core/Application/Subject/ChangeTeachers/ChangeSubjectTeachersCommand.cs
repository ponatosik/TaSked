using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record ChangeSubjectTeachersCommand(Guid UserId, Guid SubjectId, List<UpdateTeacherDTO> NewSubjectTeachers)
	: IRequest<UpdateSubjectDTO>;