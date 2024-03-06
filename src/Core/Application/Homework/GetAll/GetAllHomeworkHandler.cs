using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
		var user = _context.Users.FirstOrDefault(user => user.Id == request.UserId);
		if (user == null)
		{
			throw new Exception("No such user found");
		}
		if (user.GroupId == null)
		{
			throw new Exception("User is not a member of a group");
		}
		if(user.Role == GroupRole.Member)
		{
			throw new Exception("No permision to create subjects");
		}

		var group = _context.Groups.Include(e => e.Subjects).ThenInclude(e => e.Homeworks).FirstOrDefault(group => group.Id == user.GroupId);
		if (group == null)
		{
			throw new Exception("Unknown Group");
		}

		return Task.FromResult(group.Subjects.SelectMany(subject => subject.Homeworks, (subject, hw) => hw).ToList());
	}
}
