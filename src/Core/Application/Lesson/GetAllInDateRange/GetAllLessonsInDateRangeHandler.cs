using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class GetAllLessonsInDateRangeHandler : IRequestHandler<GetAllLessonsInDateRangeQuery, List<Lesson>>
{
    private readonly IApplicationDbContext _context;

    public GetAllLessonsInDateRangeHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Lesson>> Handle(GetAllLessonsInDateRangeQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups.Include(e => e.Subjects).ThenInclude(e => e.Lessons).FindOrThrow(user.GroupId.Value);

        var startDate = request.StartDate ?? DateTime.MinValue;
        var endDate = request.EndDate ?? DateTime.Now.AddYears(1);
        var lessonsInRange = group.Subjects
            .SelectMany(subject => subject.Lessons)
            .Where(lesson => lesson.Time >= startDate && lesson.Time <= endDate)
            .ToList();

        return Task.FromResult(lessonsInRange);
    }
}