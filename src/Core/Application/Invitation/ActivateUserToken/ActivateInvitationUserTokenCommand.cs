using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ActivateInvitationUserTokenCommand(Guid UserId, Guid InvitationId, Guid GroupId) : IRequest<string>;