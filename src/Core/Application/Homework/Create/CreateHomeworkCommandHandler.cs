using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class CreateHomeworkCommandHandler : IRequestHandler<CreateHomeworkCommand, Homework>
{
	private readonly IApplicationDbContext _context;
	private readonly IPublisher? _eventPublisher;
	
	public CreateHomeworkCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
	{
		_context = context;
		_eventPublisher = eventPublisher;
	}

	public async Task<Homework> Handle(CreateHomeworkCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindById(request.UserId);
		var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
		var subject = group.Subjects.FindById(request.SubjectId);

		var homework = subject.CreateHomework(request.Title, request.Description, request.Deadline);
		
		await _context.SaveChangesAsync(cancellationToken);
		if(_eventPublisher is not null)
		{
			await _eventPublisher.Publish(new HomeworkCreatedEvent(homework, group.Id));
		}
		return homework;
	}
}