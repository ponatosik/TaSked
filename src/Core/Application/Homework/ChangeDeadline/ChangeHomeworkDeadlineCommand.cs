using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeHomeworkDeadlineCommand(Guid UserId, Guid SubjectId, Guid HomeworkId, DateTime? HomeworkDeadline) : IRequest<Homework>;