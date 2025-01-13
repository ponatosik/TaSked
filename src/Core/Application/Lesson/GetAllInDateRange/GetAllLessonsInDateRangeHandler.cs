using MediatR;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

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
        var startDate = request.StartDate ?? DateTime.MinValue;
        var endDate = request.EndDate ?? DateTime.MaxValue;
        
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var lessonsInRange = _context.Groups
            .Where(g => g.Id == groupId)
            .SelectMany(g => g.Subjects.SelectMany(s => s.Lessons))
            .Where(lesson => lesson.Time >= startDate && lesson.Time <= endDate)
            .ToList();

        return Task.FromResult(lessonsInRange);
    }
}