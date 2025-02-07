using Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using TaSked.Domain;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("Groups/{groupId:guid}/[controller]")]
[Authorize]
public class MembersController : ControllerBase
{
	private readonly IMediator _mediator;

	public MembersController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
    [HttpGet]
    [Authorize(AccessPolicies.Moderator)]
    public async Task<IActionResult> Get(Guid groupId)
    {
        Guid userId = User.GetUserId()!.Value;
        var result = await _mediator.Send(new GetGroupMembersQuery(userId, groupId));
        return Ok(result);
    }

	[HttpPatch]
	[Authorize(AccessPolicies.Admin)]
	[Route("Promote")]
	public async Task<IActionResult> PatchPromote(Guid groupId, PromoteMemberRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new PromoteMemberCommand(userId, groupId, request.UserId, GroupRole.Moderator));
		return Ok();
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Admin)]
	[Route("Demote")]
	public async Task<IActionResult> PatchDemote(Guid groupId, DemoteMemberRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DemoteMemberCommand(userId, groupId, request.UserId, GroupRole.Member));
		return Ok();
	}

	[HttpDelete]
	[Authorize(AccessPolicies.Admin)]
	[Route("Ban")]
	public async Task<IActionResult> Delete(Guid groupId, BanMemberRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new BanMemberCommand(userId, groupId, request.UserId));
		return NoContent();
	}
}