using MediatR;
using TaSked.Application.Data;
using TaSked.Application.Exceptions;
using TaSked.Domain;

namespace TaSked.Application;

public class CreateAnnouncementCommandHandler : IRequestHandler<CreateAnnouncementCommand, Announcement>
{
    private readonly IApplicationDbContext _context;
    private readonly IPublisher? _eventPublisher;

    public CreateAnnouncementCommandHandler(IApplicationDbContext context, IPublisher? eventPublisher = null)
    {
        _context = context;
        _eventPublisher = eventPublisher;
    }

    public async Task<Announcement> Handle(CreateAnnouncementCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FindOrThrow(request.UserId);
        var groupId = user.GroupId ?? throw new UserIsNotGroupMemberException(user.Id, Guid.Empty);
        var group = _context.Groups.FindOrThrow(groupId);

        var announcement = group.CreateAnnouncement(request.AnnouncementTitle, request.AnnouncementMessage);

        await _context.SaveChangesAsync(cancellationToken);
        if(_eventPublisher != null)
        {
	        await _eventPublisher.Publish(new AnnouncementCreatedEvent(announcement, group.Id), cancellationToken);
        }

        return announcement;
    }
}