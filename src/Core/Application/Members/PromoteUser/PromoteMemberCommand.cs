using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record PromoteMemberCommand(Guid GroupId, Guid UserId, GroupRole Role) : IRequest;