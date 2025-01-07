using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class GetAllLessonsBySubjectHandler : IRequestHandler<GetAllLessonsBySubjectQuery, List<Lesson>>
{
    private readonly IApplicationDbContext _context;

    public GetAllLessonsBySubjectHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Lesson>> Handle(GetAllLessonsBySubjectQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.Include(e => e.Subjects).ThenInclude(e => e.Lessons).FindOrThrow(user.GroupId.Value);
        var lessons = group.Subjects.FindOrThrow(request.SubjectId)?.Lessons.ToList();

        return Task.FromResult(lessons);
    }
}