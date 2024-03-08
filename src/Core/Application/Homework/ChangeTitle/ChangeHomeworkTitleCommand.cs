using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeHomeworkTitleCommand(Guid UserId, Guid SubjectId, Guid HomeworkId, string HomeworkTitle) : IRequest<Homework>;