using MediatR;
using TaSked.Application.Common;

namespace TaSked.Application;

public record UpdateGroupCommand(Guid UserId, Optional<string> GroupName) : IRequest<GroupDTO>;
