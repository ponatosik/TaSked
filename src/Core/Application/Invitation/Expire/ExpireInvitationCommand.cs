using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ExpireInvitationCommand(Guid UserId, Guid InvitationId) : IRequest;