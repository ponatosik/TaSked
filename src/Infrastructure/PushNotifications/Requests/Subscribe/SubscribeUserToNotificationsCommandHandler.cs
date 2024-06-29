using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;

namespace PushNotifications.Requests;

public class SubscribeUserToNotificationsCommandHandler : IRequestHandler<SubscribeUserToNotificationsCommand>
{
	public Task Handle(SubscribeUserToNotificationsCommand request, CancellationToken cancellationToken)
	{
		var topic = Helpers.GetGroupTopicName(request.GroupId);
		var tokens = new List<string>() { request.FirebaseToken };
		return FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(tokens, topic);
	}
} 