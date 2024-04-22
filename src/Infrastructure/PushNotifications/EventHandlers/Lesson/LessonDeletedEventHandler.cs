using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;
using TaSked.Application;

namespace PushNotifications.EventHandlers;

internal class LessonDeletedEventHandler : INotificationHandler<LessonDeletedEvent>
{
	public Task Handle(LessonDeletedEvent applicationEvent, CancellationToken cancellationToken)
	{
		var topic = $"Group:{applicationEvent.GroupId}";
		var message = new Message()
		{
			Notification = Helpers.GetLessonDeletedNotification(applicationEvent.Lesson),
			Topic = topic,
		};
		return FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
	}
}
