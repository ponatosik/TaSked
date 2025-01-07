using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var group = _context.Groups.Include(group => group.Subjects).FindOrThrow(user.GroupId.Value);
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