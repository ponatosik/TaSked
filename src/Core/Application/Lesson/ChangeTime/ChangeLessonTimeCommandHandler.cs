using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ChangeLessonTimeCommandHandler : IRequestHandler<ChangeLessonTimeCommand, Lesson>
{
    private readonly IApplicationDbContext _context;

    public ChangeLessonTimeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson> Handle(ChangeLessonTimeCommand request, CancellationToken cancellationToken)
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

        var lesson = subject.Lessons.FirstOrDefault(lesson => lesson.Id == request.LessonId);
        if (lesson == null)
        {
            throw new Exception("Unknown Lesson");
        }

        lesson.Time = request.NewTime;

        await _context.SaveChangesAsync(cancellationToken);
        return lesson;
    }
}