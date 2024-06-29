using MediatR;
using TaSked.Domain;

namespace PushNotifications.Requests;

public record SubscribeUserToNotificationsCommand (User User, Guid GroupId, string FirebaseToken) : IRequest;
