using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class DeleteHomeworkCommandHandler : IRequestHandler<DeleteHomeworkCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public DeleteHomeworkCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null) 
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(DeleteHomeworkCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(group => group.Subjects)
            .ThenInclude(subject => subject.Homeworks)
            .FindOrThrow(groupId);

        var subject = group.Subjects.FindOrThrow(request.SubjectId);
        var homework = subject.Homeworks.FindOrThrow(request.HomeworkId);
        
        subject.Homeworks.Remove(homework);
        
        await _context.SaveChangesAsync(cancellationToken);
        if(_eventPublisher is not null)
        {
            await _eventPublisher.Publish(new HomeworkDeletedEvent(homework, group.Id), cancellationToken);
        }
    }
}