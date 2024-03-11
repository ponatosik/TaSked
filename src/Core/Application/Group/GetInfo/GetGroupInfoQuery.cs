using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetGroupInfoQuery(Guid GroupId) : IRequest<GroupDTO>;