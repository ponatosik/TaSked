using FirebaseAdmin.Messaging;
using MediatR;
using PushNotifications.Common;

namespace PushNotifications.Requests;

public class UnsubscribeUserFromNotificationsCommandHandler : IRequestHandler<UnsubscribeUserFromNotificationsCommand>
{
	public Task Handle(UnsubscribeUserFromNotificationsCommand request, CancellationToken cancellationToken)
	{
		var topic = Helpers.GetGroupTopicName(request.GroupId);
		var tokens = new List<string>() { request.FirebaseToken };
		return FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(tokens, topic);
	}
}
