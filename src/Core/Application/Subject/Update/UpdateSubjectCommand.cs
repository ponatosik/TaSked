using MediatR;
using TaSked.Application.Common;
using TaSked.Domain;

namespace TaSked.Application;

public record UpdateSubjectCommand(
	 Guid UserId,
	 Guid SubjectId,
	 Optional<string> SubjectName,
	 Optional<Teacher> SubjectTeacher
	): IRequest<UpdateSubjectDTO>;
