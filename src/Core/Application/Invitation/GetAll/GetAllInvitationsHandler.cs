using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class GetAllInvitationsHandler : IRequestHandler<GetAllInvitationsQuery, List<Invitation>>
{
    private readonly IApplicationDbContext _context;

    public GetAllInvitationsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Invitation>> Handle(GetAllInvitationsQuery request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(e => e.Invitations)
            .AsNoTracking()
            .FindOrThrow(groupId);

        var invitations = group.Invitations;
        return Task.FromResult(invitations);
    }
}