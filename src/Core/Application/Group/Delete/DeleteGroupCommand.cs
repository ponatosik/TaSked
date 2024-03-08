using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record DeleteGroupCommand(Guid UserId) : IRequest;