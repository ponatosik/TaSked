using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class UpdateLessonCommandHandler : IRequestHandler<UpdateLessonCommand, Lesson>
{
    private readonly IApplicationDbContext _context;

    public UpdateLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Lesson> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups
            .Include(group => group.Subjects)
            .ThenInclude(subject => subject.Lessons)
            .FindById(user.GroupId!.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var lesson = subject.Lessons.FindById(request.LessonId);

        if (request.LessonTime.IsEmpty)
        {
            return lesson;
        }

        lesson.Time = request.LessonTime.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return lesson;
    }
}