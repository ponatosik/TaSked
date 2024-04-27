using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public DeleteLessonCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher; 
    }

    public async Task Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var lesson = subject.Lessons.FindById(request.LessonId);
        
        subject.Lessons.Remove(lesson);
        
        await _context.SaveChangesAsync(cancellationToken);
        if (_eventPublisher is not null) 
        {
            await _eventPublisher.Publish(new LessonDeletedEvent(lesson, group.Id), cancellationToken);
        }
    }
}