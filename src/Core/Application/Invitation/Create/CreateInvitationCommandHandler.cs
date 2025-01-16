using MediatR;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, Invitation>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public CreateInvitationCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task<Invitation> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups.FindOrThrow(groupId);

        var invitation =
	        group.CreateInvitation(request.InvitationCaption, request.MaxActivations, request.ExpirationDate);

        await _context.SaveChangesAsync(cancellationToken);
        if (_eventPublisher is not null)
        {
            await _eventPublisher.Publish(new InvitationCreatedEvent(invitation, group.Id), cancellationToken);
        }
        return invitation;
    }
}