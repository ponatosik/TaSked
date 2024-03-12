using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record PromoteMemberCommand(Guid PromotedBy, Guid GroupId, Guid UserId, GroupRole Role) : IRequest;