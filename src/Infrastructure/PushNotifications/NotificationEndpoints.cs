using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PushNotifications.Requests;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Infrastructure.PushNotifications.Endpoints;

public static class NotificationsEndpoints
{
	public static void MapNotificationsEndpoints(this IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/Notifications")
			.RequireAuthorization()
			.WithTags("Notifications");

		group.MapPost("/Subscribe",
				async (HttpContext context, IMediator mediator, SubscribeToNotificationsRequest request) =>
				{
					var userId = context.User.GetUserId()!.Value;
					var user = await mediator.Send(new GetUserInfoQuery(userId));
					var groupId = user.GroupId!.Value;

					await mediator.Send(
						new SubscribeUserToNotificationsCommand(user, groupId, request.FirebaseDeviceToken));
					return Results.Ok();
				})
			.RequireAuthorization(AccessPolicies.Member)
			.WithName("Subscribe to notifications");

		group.MapPost("/Unsubscribe",
				async (HttpContext context, IMediator mediator, UnsubscribeFromNotificationsRequest request) =>
				{
					var userId = context.User.GetUserId()!.Value;
					var user = await mediator.Send(new GetUserInfoQuery(userId));
					var groupId = user.GroupId!.Value;

					await mediator.Send(
						new UnsubscribeUserFromNotificationsCommand(user, groupId, request.FirebaseDeviceToken));
					return Results.Ok();
				})
			.RequireAuthorization(AccessPolicies.Member)
			.WithName("Unsubscribe from notifications");
	}
}