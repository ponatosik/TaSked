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
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(e => e.Subjects).FindById(user.GroupId.Value);
        
        return Task.FromResult(group.Subjects);
    }
}