using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record AnnouncementCreatedEvent(Announcement Announcement, Guid GroupId) : INotification;