using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class CommentHomeworkCommandHandler : IRequestHandler<CommentHomeworkCommand, Comment>
{
	private readonly IApplicationDbContext _context;

	public CommentHomeworkCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Comment> Handle(CommentHomeworkCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindOrThrow(request.UserId);
		var group = _context.Groups
			.Include(group => group.Subjects)
			.ThenInclude(subject => subject.Homeworks)
			.FindOrThrow(user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty));
		var subject = group.Subjects.FindOrThrow(request.SubjectId);
		var homework = subject.Homeworks.FindOrThrow(request.HomeworkId);

		var comment = homework.LeaveComment(user, request.Content);

		await _context.SaveChangesAsync(cancellationToken);
		return comment;
	}
}