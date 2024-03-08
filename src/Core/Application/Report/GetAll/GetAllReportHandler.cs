using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class GetAllReportHandler : IRequestHandler<GetAllReportQuery, List<Report>>
{
    private readonly IApplicationDbContext _context;

    public GetAllReportHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Report>> Handle(GetAllReportQuery request, CancellationToken cancellationToken)
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

        var group = _context.Groups.FirstOrDefault(group => group.Id == user.GroupId);
        if (group == null)
        {
            throw new Exception("Unknown Group");
        }

        return Task.FromResult(group.Reports.ToList());
    }
}