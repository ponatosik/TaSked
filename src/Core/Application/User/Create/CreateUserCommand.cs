using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateUserCommand(string Nickname) : IRequest<User>;
