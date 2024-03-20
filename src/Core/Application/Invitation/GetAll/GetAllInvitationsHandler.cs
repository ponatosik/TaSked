using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
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
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(e => e.Invitations).FindById(user.GroupId.Value);

        var invitations = group.Invitations;
        return Task.FromResult(invitations);
    }
}