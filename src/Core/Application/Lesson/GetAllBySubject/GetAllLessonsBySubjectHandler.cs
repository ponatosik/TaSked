using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

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
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var subject = _context.Groups
            .Where(g => g.Id == groupId)
            .Include(g => g.Subjects)
                .ThenInclude(s => s.Lessons)
            .Select(g => g.Subjects.FirstOrDefault(s => s.Id == request.SubjectId))
            .Select(s => new { s!.Lessons })
            .FirstOrDefault() ?? throw new EntityNotFoundException(request.SubjectId, nameof(Subject));

        if (subject.Lessons is null) throw new EntityNotFoundException(request.SubjectId, nameof(Subject));

        return Task.FromResult(subject.Lessons);
    }
}