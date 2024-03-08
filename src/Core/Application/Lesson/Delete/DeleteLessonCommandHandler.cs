using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class DeleteLessonCommandHandler : IRequestHandler<DeleteLessonCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLessonCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(group => group.Subjects).FindById(user.GroupId.Value);
        var subject = group.Subjects.FindById(request.SubjectId);
        var lesson = subject.Lessons.FindById(request.LessonId);
        
        subject.Lessons.Remove(lesson);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}