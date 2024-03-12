using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record DemoteMemberCommand(Guid PromotedBy, Guid GroupId, Guid UserId, GroupRole Role) : IRequest;