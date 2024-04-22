using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;
using TaSked.Application;

namespace PushNotifications.EventHandlers;

internal class SubjectDeletedEventHandler : INotificationHandler<SubjectDeletedEvent>
{
	public Task Handle(SubjectDeletedEvent applicationEvent, CancellationToken cancellationToken)
	{
		var topic = Helpers.GetGroupTopicName(applicationEvent.GroupId);
		var message = new Message()
		{
			Notification = Helpers.GetSubjectDeletedNotification(applicationEvent.Subject),
			Topic = topic,
		};
		return FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
	}
}
