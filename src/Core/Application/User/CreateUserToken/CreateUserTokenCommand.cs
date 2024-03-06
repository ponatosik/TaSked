using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateUserTokenCommand(string Nickname) : IRequest<string>;
