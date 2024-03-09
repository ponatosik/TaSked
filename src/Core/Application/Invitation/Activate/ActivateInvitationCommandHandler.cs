using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TaSked.Application;

public class ActivateInvitationCommandHandler : IRequestHandler<ActivateInvitationCommand>
{
    private readonly IApplicationDbContext _context;

    public ActivateInvitationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivateInvitationCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindById(request.UserId);
        var group = _context.Groups.Include(x => x.Invitations).FindById(request.GroupId);
        var invitation = group.Invitations.FindById(request.InvitationId);

        group.JoinByInvintation(invitation, user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}