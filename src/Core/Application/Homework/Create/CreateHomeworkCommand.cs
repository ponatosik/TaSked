using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record CreateHomeworkCommand(Guid UserId, Guid SubjectId, string Title, string Description) : IRequest<Homework>;
