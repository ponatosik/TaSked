using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class CommentSubjectCommandHandler : IRequestHandler<CommentSubjectCommand, Comment>
{
	private readonly IApplicationDbContext _context;

	public CommentSubjectCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Comment> Handle(CommentSubjectCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindOrThrow(request.UserId);
		var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
		var group = _context.Groups
			.Include(g => g.Subjects)
			.FindOrThrow(groupId);
		var subject = group.Subjects.FindOrThrow(request.SubjectId);

		var comment = subject.LeaveComment(user, request.Content);

		await _context.SaveChangesAsync(cancellationToken);
		return comment;
	}
}