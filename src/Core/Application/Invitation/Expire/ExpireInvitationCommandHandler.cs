using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ExpireInvitationCommandHandler : IRequestHandler<ExpireInvitationCommand>
{
    private readonly IApplicationDbContext _context;

    public ExpireInvitationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ExpireInvitationCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups
            .Include(g => g.Invitations)
            .FindOrThrow(groupId);
        var invitation = group.Invitations.FindOrThrow(request.InvitationId);

        invitation.Expire();
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}