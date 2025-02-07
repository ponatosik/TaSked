using MediatR;
using TaSked.Domain;

namespace TaSked.Application;

public record CreateAnnouncementCommand(Guid UserId, string AnnouncementTitle, string AnnouncementMessage)
	: IRequest<Announcement>;