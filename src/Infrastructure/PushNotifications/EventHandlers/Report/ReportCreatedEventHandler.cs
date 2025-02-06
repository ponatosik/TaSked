using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;
using TaSked.Application;

namespace PushNotifications.EventHandlers;

internal class ReportCreatedEventHandler : INotificationHandler<AnnouncementCreatedEvent>
{
	public Task Handle(AnnouncementCreatedEvent applicationEvent, CancellationToken cancellationToken)
	{
		var topic = Helpers.GetGroupTopicName(applicationEvent.GroupId);
		var message = new Message()
		{
			Notification = Helpers.GetReportCreatedNotification(applicationEvent.Announcement),
			Topic = topic,
		};
		return FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
	}
}
