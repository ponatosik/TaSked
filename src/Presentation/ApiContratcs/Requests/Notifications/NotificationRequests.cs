namespace TaSked.Api.Requests;

public record SubscribeToNotificationsRequest (string FirebaseDeviceToken);
public record UnsubscribeFromNotificationsRequest (string FirebaseDeviceToken);
