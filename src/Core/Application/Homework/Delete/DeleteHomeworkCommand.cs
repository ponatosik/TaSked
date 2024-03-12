using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record DeleteHomeworkCommand(Guid UserId, Guid SubjectId, Guid HomeworkId) : IRequest;