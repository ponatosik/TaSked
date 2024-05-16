using Plugin.Firebase.CloudMessaging;
using TaSked.Api.ApiClient.Notifications;
using TaSked.Api.Requests;

namespace TaSked.App.Common.Notifications;

public class NotificationsService
{
	private readonly ITaSkedNotifications _notificationsService;

    public NotificationsService(ITaSkedNotifications notificationsService)
    {
        _notificationsService = notificationsService; 
    }

	public async Task SubscribeToNotifications()
	{
		var request = new SubscribeToNotificationsRequest(await GetFirebaseToken());
		await _notificationsService.SubscribeToNotifications(request);
	}

	public async Task UnsubscribeFromNotifications()
	{
		var request = new UnsubscribeFromNotificationsRequest(await GetFirebaseToken());
		await _notificationsService.UnsubscribeFromNotifications(request);
	}

	private static async Task<string> GetFirebaseToken()
	{
		await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
		return await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
	}
}
