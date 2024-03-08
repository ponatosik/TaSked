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
		var user = _context.Users.FindById(request.UserId);
		var group = _context.Groups.Include(e => e.Subjects).ThenInclude(e => e.Homeworks).FindById(user.GroupId.Value);

		return Task.FromResult(group.Subjects.SelectMany(subject => subject.Homeworks, (subject, hw) => hw).ToList());
	}
}
