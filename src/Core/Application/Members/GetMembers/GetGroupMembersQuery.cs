using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetGroupMembersQuery(Guid UserId, Guid GroupId) : IRequest<List<User>>;