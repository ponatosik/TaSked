using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Lesson>
{
    private readonly IApplicationDbContext _context;

    public CreateLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
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
            throw new Exception("Unkcnown Subject");
        }

        var lesson = subject.CreateLesson(request.LessonTime);

        await _context.SaveChangesAsync(cancellationToken);
        return lesson;
    }
}