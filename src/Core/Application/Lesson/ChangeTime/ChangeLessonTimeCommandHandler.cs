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
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups
            .Include(group => group.Subjects)
            .ThenInclude(subject => subject.Lessons)
            .FindOrThrow(user.GroupId.Value);
        var subject = group.Subjects.FindOrThrow(request.SubjectId);
        var lesson = subject.Lessons.FindOrThrow(request.LessonId);

        lesson.Time = request.NewTime;

        await _context.SaveChangesAsync(cancellationToken);
        return lesson;
    }
}