using TaSked.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using Microsoft.AspNetCore.Authorization;
using TaSked.Infrastructure.Authorization;
using TaSked.Api.Requests;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AccessPolicise.Member)]
public class InvitationController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvitationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Post(CreateInvintationRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new CreateInvitationCommand(userId, request.InvitationCaption)));
    }

    [HttpPost]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Post(ActivateInvintationRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new ActivateInvitationCommand(userId, request.InvitationId, request.GroupId));
        return Ok();
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Patch(ExpireInvintationRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new ExpireInvitationCommand(userId, request.InvitationId));
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(GetInvintationInfoRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetInvitationInfoQuery(request.InvitationId)));
    }
}