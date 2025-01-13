using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

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
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(g => g.Reports)
            .AsNoTracking()
            .FindOrThrow(groupId);

        return Task.FromResult(group.Reports.ToList());
    }
}