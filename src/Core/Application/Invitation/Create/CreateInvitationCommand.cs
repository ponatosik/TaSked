using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateInvitationCommand(Guid UserId, string? InvitationCaption, int? MaxActivations = null, DateTime? ExpirationDate = null) : IRequest<Invitation>;