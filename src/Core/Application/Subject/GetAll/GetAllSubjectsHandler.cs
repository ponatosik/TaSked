using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
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
        var group = _context.Groups
            .Include(e => e.Subjects)
            .ThenInclude(e => e.Homeworks)
            .Include(e => e.Subjects)
            .ThenInclude(e => e.Lessons)
            .FindOrThrow(user.GroupId.Value);

        var subjects = group.Subjects;
        return Task.FromResult(subjects.Select(subject => SubjectDTO.From(subject)).ToList());
    }
}