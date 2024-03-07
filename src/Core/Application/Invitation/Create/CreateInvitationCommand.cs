using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateInvitationCommand(Guid UserId, string? InvitationCaption) : IRequest<Invitation>;