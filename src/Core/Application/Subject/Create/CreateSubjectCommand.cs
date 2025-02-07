using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record CreateSubjectCommand(
	Guid UserId,
	string SubjectName,
	List<Teacher>? Teachers = null,
	List<RelatedLink>? RelatedLinks = null) : IRequest<SubjectDTO>;
