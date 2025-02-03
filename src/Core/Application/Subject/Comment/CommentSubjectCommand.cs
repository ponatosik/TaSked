using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record CommentSubjectCommand(Guid UserId, Guid SubjectId, string Content) : IRequest<Comment>;