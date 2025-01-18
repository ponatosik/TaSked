using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvitationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Post(CreateInvintationRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new CreateInvitationCommand(userId, request.InvitationCaption, request.MaxActivations, request.ExpirationDate));
        return CreatedAtAction(nameof(Get), new { InvitationId = result.Id }, result);
    }

    [HttpPost]
    [Route("Activate")]
    public async Task<IActionResult> Post(ActivateInvintationRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new ActivateInvitationCommand(userId, request.InvitationId, request.GroupId));
        return Ok();
    }

    [HttpPatch]
    [Authorize(AccessPolicies.Moderator)]
    [Route("Expire")]
    public async Task<IActionResult> Patch(ExpireInvintationRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new ExpireInvitationCommand(userId, request.InvitationId));
        return Ok();
    }

    [HttpGet]
    [Route("{InvitationId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid InvitationId)
    {
        var result = await _mediator.Send(new GetInvitationInfoQuery(InvitationId));
        return Ok(result);
    }

	[HttpGet]
	[Authorize(AccessPolicies.Admin)]
	public async Task<IActionResult> Get()
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetAllInvitationsQuery(userId));
		return Ok(result);
	}
}