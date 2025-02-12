using Plugin.Firebase.CloudMessaging;
using TaSked.Api.ApiClient.Notifications;
using TaSked.Api.Requests;

namespace TaSked.App.Common.Notifications;

public class NotificationsService
{
	private readonly ITaSkedNotifications _notificationsService;
	private readonly LoginService _loginService;

	public NotificationsService(ITaSkedNotifications notificationsService, LoginService loginService)
    {
	    _notificationsService = notificationsService;
	    _loginService = loginService;

	    _loginService.GroupJoined += async () => await SubscribeToNotifications();
	    _loginService.GroupLeft += async () => await UnsubscribeFromNotifications();
	    _loginService.LogoutCompleted += async () => await UnsubscribeFromNotifications();
	    _loginService.LoginCompleted += async () =>
	    {
		    if (await loginService.GetGroupIdAsync() is not null)
		    {
			    await SubscribeToNotifications();
		    }
	    };
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
