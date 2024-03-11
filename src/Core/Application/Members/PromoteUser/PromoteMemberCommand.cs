using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record PromoteMemberCommand(Guid promotedBy, Guid GroupId, Guid UserId, GroupRole Role) : IRequest;