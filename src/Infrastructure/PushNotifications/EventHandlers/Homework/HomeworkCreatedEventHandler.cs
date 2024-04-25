using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;
using TaSked.Application;

namespace PushNotifications.EventHandlers;

internal class HomeworkCreatedEventHandler : INotificationHandler<HomeworkCreatedEvent>
{
	public Task Handle(HomeworkCreatedEvent applicationEvent, CancellationToken cancellationToken)
	{
		var topic = Helpers.GetGroupTopicName(applicationEvent.GroupId);
		var message = new Message()
		{
			Notification = Helpers.GetHomeworkCreatedNotification(applicationEvent.Homework),
			Topic = topic,
		};
		return FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
	}
}
