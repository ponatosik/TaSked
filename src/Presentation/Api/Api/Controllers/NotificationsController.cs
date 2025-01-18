using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PushNotifications.Requests;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("Notifications/")]
[Authorize]
public class NotificationsController : ControllerBase
{
	private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

	[HttpPost]
	[Authorize(AccessPolicies.Member)]
	[Route("Subscribe")]
	public async Task<IActionResult> Subscribe([FromBody] SubscribeToNotificationsRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var user = await _mediator.Send(new GetUserInfoQuery(userId));
		var groupId = user.GroupId!.Value;
        await _mediator.Send(new SubscribeUserToNotificationsCommand(user, groupId, request.FirebaseDeviceToken));
		return Ok();
	}

	[HttpPost]
	[Authorize(AccessPolicies.Member)]
	[Route("Unsubscribe")]
	public async Task<IActionResult> Unsubscribe([FromBody] UnsubscribeFromNotificationsRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var user = await _mediator.Send(new GetUserInfoQuery(userId));
		var groupId = user.GroupId!.Value;
        await _mediator.Send(new UnsubscribeUserFromNotificationsCommand(user, groupId, request.FirebaseDeviceToken));
		return Ok();
	}
}
