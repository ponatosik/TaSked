using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record CreateSubjectCommand(
	Guid UserId,
	string SubjectName,
	Teacher? Teacher = null,
	List<RelatedLink>? RelatedLinks = null) : IRequest<SubjectDTO>;
