using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record ChangeSubjectRelatedLinksCommand(Guid UserId, Guid SubjectId, List<RelatedLink> RelatedLinks)
	: IRequest<UpdateSubjectDTO>;