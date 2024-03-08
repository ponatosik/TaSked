using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ChangeHomeworkTitleCommandHandler : IRequestHandler<ChangeHomeworkTitleCommand, Homework>
{
    private readonly IApplicationDbContext _context;

    public ChangeHomeworkTitleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Homework> Handle(ChangeHomeworkTitleCommand request, CancellationToken cancellationToken)
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
            throw new Exception("No permission to create subjects");
        }

        var group = _context.Groups.Include(group => group.Subjects).FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        var subject = group.Subjects.FirstOrDefault(subject => subject.Id == request.SubjectId);
        if (subject == null)
        {
            throw new Exception("Unknown Subject");
        }

        var homework = subject.Homeworks.FirstOrDefault(homework => homework.Id == request.HomeworkId);
        if (homework == null)
        {
            throw new Exception("Unknown Homework");
        }

        homework.Title = request.HomeworkTitle;

        await _context.SaveChangesAsync(cancellationToken);
        return homework;
    }
}