using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetInvitationInfoQuery(Guid InvitationId) : IRequest<Invitation>;