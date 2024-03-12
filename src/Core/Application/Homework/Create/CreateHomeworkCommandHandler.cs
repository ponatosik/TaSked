using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class CreateHomeworkCommandHandler : IRequestHandler<CreateHomeworkCommand, Homework>
{
	private readonly IApplicationDbContext _context;
	
	public CreateHomeworkCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Homework> Handle(CreateHomeworkCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindById(request.UserId);
		var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
		var subject = group.Subjects.FindById(request.SubjectId);

		var homework = subject.CreateHomework(request.Title, request.Description);
		
		await _context.SaveChangesAsync(cancellationToken);
		return homework;
	}
}