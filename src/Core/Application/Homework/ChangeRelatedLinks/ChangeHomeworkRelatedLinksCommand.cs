using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record ChangeHomeworkRelatedLinksCommand(
	Guid UserId,
	Guid SubjectId,
	Guid HomeworkId,
	List<RelatedLink> Links) : IRequest<Homework>;