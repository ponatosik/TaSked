using MediatR;
using TaSked.Domain;

namespace PushNotifications.Requests;

public record UnsubscribeUserFromNotificationsCommand (User User, Guid GroupId, string FirebaseToken) : IRequest;
