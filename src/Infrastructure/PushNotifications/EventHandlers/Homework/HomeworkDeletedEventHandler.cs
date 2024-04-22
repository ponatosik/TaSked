using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;
using TaSked.Application;

namespace PushNotifications.EventHandlers;

internal class HomeworkDeletedEventHandler : INotificationHandler<HomeworkDeletedEvent>
{
	public Task Handle(HomeworkDeletedEvent applicationEvent, CancellationToken cancellationToken)
	{
		var topic = $"Group:{applicationEvent.GroupId}";
		var message = new Message()
		{
			Notification = Helpers.GetHomeworkDeletedNotification(applicationEvent.Homework),
			Topic = topic,
		};
		return FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
	}
}
