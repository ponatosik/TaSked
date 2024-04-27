using Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using TaSked.Domain;
using TaSked.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using PushNotifications.Requests;
using TaSked.Api.Requests;

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
	[Authorize(AccessPolicise.Member)]
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
	[Authorize(AccessPolicise.Member)]
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
