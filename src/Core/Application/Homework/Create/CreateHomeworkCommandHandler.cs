﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

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
		var user = _context.Users.FindOrThrow(request.UserId);
		var group = _context.Groups
			.Include(group => group.Subjects.Where(s => s.Id == request.SubjectId))
			.FindOrThrow(user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty));
		var subject = group.Subjects.FindOrThrow(request.SubjectId);

		var homework =
			subject.CreateHomework(request.Title, request.Description, request.Deadline, request.RelatedLinks);
		
		await _context.SaveChangesAsync(cancellationToken);
		if(_eventPublisher is not null)
		{
			await _eventPublisher.Publish(new HomeworkCreatedEvent(homework, group.Id), cancellationToken);
		}
		return homework;
	}
}