using MediatR;

namespace TaSked.Application;

public record ActivateInvitationCommand(Guid UserId, Guid InvitationId, Guid GroupId) : IRequest;