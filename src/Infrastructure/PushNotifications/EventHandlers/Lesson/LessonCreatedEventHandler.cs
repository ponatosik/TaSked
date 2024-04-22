using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;
using TaSked.Application;

namespace PushNotifications.EventHandlers;

internal class LessonCreatedEventHandler : INotificationHandler<LessonCreatedEvent>
{
	public Task Handle(LessonCreatedEvent applicationEvent, CancellationToken cancellationToken)
	{
		var topic = $"Group:{applicationEvent.GroupId}";
		var message = new Message()
		{
			Notification = Helpers.GetLessonCreatedNotification(applicationEvent.Lesson),
			Topic = topic,
		};
		return FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
	}
}
