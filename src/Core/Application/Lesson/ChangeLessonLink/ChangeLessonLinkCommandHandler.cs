using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

public class ChangeLessonLinkCommandHandler : IRequestHandler<ChangeLessonLinkCommand, Lesson>
{
	private readonly IApplicationDbContext _context;

	public ChangeLessonLinkCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Lesson> Handle(ChangeLessonLinkCommand request, CancellationToken cancellationToken)
	{
		var user = _context.Users.FindOrThrow(request.UserId);
		var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
		var group = _context.Groups
			.Include(group => group.Subjects)
			.ThenInclude(subject => subject.Lessons)
			.FindOrThrow(groupId);
		var subject = group.Subjects.FindOrThrow(request.SubjectId);
		var lesson = subject.Lessons.FindOrThrow(request.LessonId);

		lesson.OnlineLessonUrl = request.LessonLink;

		await _context.SaveChangesAsync(cancellationToken);
		return lesson;
	}
}