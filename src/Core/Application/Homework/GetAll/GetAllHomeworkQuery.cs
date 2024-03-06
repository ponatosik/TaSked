using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record GetAllHomeworkQuery(Guid UserId) : IRequest<List<Homework>>;


