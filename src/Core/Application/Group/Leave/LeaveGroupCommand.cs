using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record LeaveGroupCommand(Guid UserId) : IRequest;