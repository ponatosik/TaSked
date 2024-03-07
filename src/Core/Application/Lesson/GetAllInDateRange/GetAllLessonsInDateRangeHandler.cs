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

        var group = _context.Groups.Include(e => e.Subjects).ThenInclude(e => e.Homeworks).FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        var startDate = request.StartDate;
        var endDate = request.EndDate;
        var lessonsInRange = group.Subjects
            .SelectMany(subject => subject.Lessons)
            .Where(lesson => lesson.Time >= startDate && lesson.Time <= endDate)
            .ToList();

        return Task.FromResult(lessonsInRange);
    }
}