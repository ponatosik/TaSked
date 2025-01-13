using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class GetAllHomeworkHandler : IRequestHandler<GetAllHomeworkQuery, List<Homework>>
{
	private readonly IApplicationDbContext _context;

	public GetAllHomeworkHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public Task<List<Homework>> Handle(GetAllHomeworkQuery request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindOrThrow(request.UserId);
		var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
		var homework = _context.Groups
			.Where(g => g.Id == groupId)
			.SelectMany(g => g.Subjects.SelectMany(s => s.Homeworks))
			.AsNoTracking()
			.ToList();

		return Task.FromResult(homework);
	}
}
