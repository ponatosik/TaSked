using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public DeleteSubjectCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(group => group.Subjects)
            .FindOrThrow(groupId);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);
        
        group.Subjects.Remove(subject);
        
        await _context.SaveChangesAsync(cancellationToken);
        if (_eventPublisher is not null)
        {
            var deletedSubject = SubjectDTO.From(subject);
            await _eventPublisher.Publish(new SubjectDeletedEvent(deletedSubject, group.Id) ,cancellationToken);
        }
    }
}