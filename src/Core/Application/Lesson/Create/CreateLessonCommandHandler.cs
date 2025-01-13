using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Lesson>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public CreateLessonCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task<Lesson> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(group => group.Subjects.Where(s => s.Id == request.SubjectId))
            .FindOrThrow(groupId);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);

        var lesson = subject.CreateLesson(request.LessonTime);

        await _context.SaveChangesAsync(cancellationToken);
        if (_eventPublisher is not null)
        {
            await _eventPublisher.Publish(new LessonCreatedEvent(lesson, group.Id), cancellationToken);
        }
        return lesson;
    }
}