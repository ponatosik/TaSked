using MediatR;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;

namespace TaSked.Application;

public class ActivateInvitationCommandHandler : IRequestHandler<ActivateInvitationCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public ActivateInvitationCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(ActivateInvitationCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var group = _context.Groups
            .Include(x => x.Invitations)
            .FindOrThrow(request.GroupId);
        var invitation = group.Invitations.FindOrThrow(request.InvitationId);

        group.JoinByInvitation(invitation, user);

        await _context.SaveChangesAsync(cancellationToken);
        if(_eventPublisher is not null)
        {
            await _eventPublisher.Publish(new InvitationActivatedEvent(invitation, user, group.Id), cancellationToken);
        }
    }
}