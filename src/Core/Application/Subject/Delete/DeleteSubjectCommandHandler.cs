using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

namespace TaSked.Application;

public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, Subject>
{
    private readonly IApplicationDbContext _context;

    public DeleteSubjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subject> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == request.UserId);
        if (user == null)
        {
            throw new Exception("No such user found");
        }
        if (user.GroupId == null)
        {
            throw new Exception("User is not a member of a group");
        }
        if (user.Role == GroupRole.Member)
        {
            throw new Exception("No permision to create subjects");
        }

        var group = _context.Groups.FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        var subject = group.Subjects.FirstOrDefault(subject => subject.Id == request.SubjectId);
        if (subject != null)
        {
            group.Subjects.Remove(subject);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return subject;
    }
}