using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetInvitationInfoQuery(Guid UserId) : IRequest<List<Invitation>>;