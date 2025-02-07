using MediatR;

namespace TaSked.Application;

public record GetSubjectCommentsQuery(Guid UserId, Guid SubjectId) : IRequest<List<CommentDTO>>;