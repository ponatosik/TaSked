using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application;

public class GetAllSubjectsHandler : IRequestHandler<GetAllSubjectsQuery, List<Subject>>
{
    private readonly IApplicationDbContext _context;

    public GetAllSubjectsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Subject>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
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

        var group = _context.Groups.Include(e => e.Subjects).FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        return Task.FromResult(group.Subjects);
    }
}