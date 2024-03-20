using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetAllInvitationsQuery(Guid UserId) : IRequest<List<Invitation>>;