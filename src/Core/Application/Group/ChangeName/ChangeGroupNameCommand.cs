using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeGroupNameCommand(Guid UserId, string GroupName) : IRequest<Group>;