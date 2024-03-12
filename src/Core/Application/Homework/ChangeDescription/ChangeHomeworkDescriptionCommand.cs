using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public record ChangeHomeworkDescriptionCommand(Guid UserId, Guid SubjectId, Guid HomeworkId, string HomeworkDescription) : IRequest<Homework>;