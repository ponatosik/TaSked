using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record CommentHomeworkCommand(Guid UserId, Guid SubjectId, Guid HomeworkId, string Content) : IRequest<Comment>;