using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class GetSubjectCommentsHandler : IRequestHandler<GetSubjectCommentsQuery, List<CommentDTO>>
{
	private readonly IApplicationDbContext _context;

	public GetSubjectCommentsHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public Task<List<CommentDTO>> Handle(GetSubjectCommentsQuery request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindOrThrow(request.UserId);
		var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);

		var subjects = _context.Groups
			.Where(g => g.Id == groupId)
			.SelectMany(g => g.Subjects.Where(s => s.Id == request.SubjectId))
			.SelectMany(s => s.Comments, (_, c) =>
				new CommentDTO(c.Id, c.Author.Nickname, c.Content)
			)
			.AsNoTracking()
			.ToList();

		return Task.FromResult(subjects);
	}
}