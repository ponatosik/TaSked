using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeHomeworkSourceUrlCommand(Guid UserId, Guid SubjectId, Guid HomeworkId, string? HomeworkSourceUrl) : IRequest<Homework>;