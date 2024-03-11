using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record DemoteMemberCommand(Guid promotedBy, Guid GroupId, Guid UserId, GroupRole role) : IRequest;