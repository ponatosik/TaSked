using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Domain.Exceptions;
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
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(g => g.Invitations).FindById(user.GroupId.Value);
        var invitation = group.Invitations.FindById(request.InvitationId);

        invitation.Expire();
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}