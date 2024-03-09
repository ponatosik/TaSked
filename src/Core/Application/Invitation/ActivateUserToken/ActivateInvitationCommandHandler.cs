using TaSked.Application.Data;
using TaSked.Domain;
using MediatR;

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
        var group = _context.Groups.FindById(request.GroupId);
        var invitation = group.Invitations.FindById(request.InvitationId);

        group.JoinByInvintation(invitation, user);

        await _context.SaveChangesAsync(cancellationToken);
    }
}