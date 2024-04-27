using Refit;
using TaSked.Api.Requests;

namespace TaSked.Api.ApiClient.Notifications;

public interface ITaSkedNotifications
{
	[Post("/Notifications/Subscribe")]
	public Task SubscribeToNotifications ([Body] SubscribeToNotificationsRequest request);

	[Delete("/Notifications/Unsubscribe")]
	public Task UnsubscribeFromNotifications([Body] UnsubscribeFromNotificationsRequest request);
}
