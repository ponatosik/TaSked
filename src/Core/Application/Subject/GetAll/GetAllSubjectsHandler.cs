using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class GetAllSubjectsHandler : IRequestHandler<GetAllSubjectsQuery, List<SubjectDTO>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSubjectsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<SubjectDTO>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        
        var subjects = _context.Groups
            .Where(g => g.Id == groupId)
            .Include(e => e.Subjects)
                .ThenInclude(e => e.Homeworks)
            .Include(e => e.Subjects)
                .ThenInclude(e => e.Lessons)
            .SelectMany(g => g.Subjects, (_, s) => 
                new SubjectDTO(
                    s.Id,
                    s.GroupId,
                    s.Name,
                    s.Homeworks.Count,
                    s.Lessons.Count,
                    s.Teacher)
            )
            .AsNoTracking()
            .ToList();

        return Task.FromResult(subjects);
    }
}