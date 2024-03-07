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

        var lessons = group.Subjects.FirstOrDefault(subject => subject.Id == request.SubjectId)?.Lessons.ToList();

        if (lessons == null)
        {
            throw new Exception("Subject not found in the group");
        }

        return Task.FromResult(lessons);
    }
}