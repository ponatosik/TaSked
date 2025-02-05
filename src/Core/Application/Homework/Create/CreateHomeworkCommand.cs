using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record CreateHomeworkCommand(
	Guid UserId,
	Guid SubjectId,
	string Title,
	string Description,
	DateTime? Deadline = null,
	List<RelatedLink>? RelatedLinks = null,
	string? BriefSummary = null) : IRequest<Homework>;
