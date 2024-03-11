using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record GetUserInfoQuery(Guid UserId) : IRequest<User>;