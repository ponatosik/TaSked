using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateGroupCommand(Guid CreatorId, string GroupName) : IRequest<Group>;
