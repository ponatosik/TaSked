using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class GetAllAnnouncementHandler : IRequestHandler<GetAllAnnouncementsQuery, List<Announcement>>
{
    private readonly IApplicationDbContext _context;

    public GetAllAnnouncementHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Announcement>> Handle(GetAllAnnouncementsQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
	        .Include(g => g.Announcements)
            .AsNoTracking()
            .FindOrThrow(groupId);

        return Task.FromResult(group.Announcements.ToList());
    }
}