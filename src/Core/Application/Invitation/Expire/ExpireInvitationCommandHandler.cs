using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Domain.Exceptions;

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

        var invitation = group.Invitations.FirstOrDefault(invitation => invitation.Id == request.InvitationId);
        if (invitation == null)
        {
            throw new Exception("Unknown Invitation");
        }

        invitation.Expire();
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}