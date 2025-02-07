using MediatR;

namespace TaSked.Application;

public record GetHomeworkCommentsQuery(Guid UserId, Guid SubjectId, Guid HomeworkId) : IRequest<List<CommentDTO>>;