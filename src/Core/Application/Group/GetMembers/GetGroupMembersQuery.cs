using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetGroupMembersQuery(Guid UserId) : IRequest<List<User>>;