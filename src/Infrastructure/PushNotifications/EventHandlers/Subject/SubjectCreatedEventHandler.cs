using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;
using TaSked.Application;

namespace PushNotifications.EventHandlers;

internal class SubjectCreatedEventHandler : INotificationHandler<SubjectCreatedEvent>
{
	public Task Handle(SubjectCreatedEvent applicationEvent, CancellationToken cancellationToken)
	{
		var topic = Helpers.GetGroupTopicName(applicationEvent.GroupId);
		var message = new Message()
		{
			Notification = Helpers.GetSubjectCreatedNotification(applicationEvent.Subject),
			Topic = topic,
		};
		return FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
	}
}
