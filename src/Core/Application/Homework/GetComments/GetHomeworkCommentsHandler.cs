using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class GetHomeworkCommentsHandler : IRequestHandler<GetHomeworkCommentsQuery, List<CommentDTO>>
{
	private readonly IApplicationDbContext _context;

	public GetHomeworkCommentsHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public Task<List<CommentDTO>> Handle(GetHomeworkCommentsQuery request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindOrThrow(request.UserId);
		var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
		var comments = _context.Groups
			.Where(g => g.Id == groupId)
			.SelectMany(g => g.Subjects.Where(s => s.Id == request.SubjectId))
			.SelectMany(s => s.Homeworks.Where(h => h.Id == request.HomeworkId))
			.SelectMany(h => h.Comments, (_, c) => new CommentDTO(c.Id, c.Author.Nickname, c.Content))
			.AsNoTracking()
			.ToList();

		return Task.FromResult(comments);
	}
}