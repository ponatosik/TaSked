using Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using TaSked.Domain;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("Groups/{GroupId:guid}/[controller]")]
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
    public async Task<IActionResult> Get(Guid GroupId)
    {
        Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetGroupMembersQuery(userId, GroupId));
        return Ok(result);
    }

	[HttpPatch]
	[Authorize(AccessPolicies.Admin)]
	[Route("Promote")]
	public async Task<IActionResult> PatchPromote(Guid GroupId, PromoteMemberRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new PromoteMemberCommand(userId, GroupId, request.UserId, GroupRole.Moderator));
		return Ok();
	}

	[HttpPatch]
	[Authorize(AccessPolicies.Admin)]
	[Route("Demote")]
	public async Task<IActionResult> PatchDemote(Guid GroupId, PromoteMemberRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DemoteMemberCommand(userId, GroupId, request.UserId, GroupRole.Member));
		return Ok();
	}

	[HttpDelete]
	[Authorize(AccessPolicies.Admin)]
	[Route("Ban")]
	public async Task<IActionResult> Delete(Guid GroupId, BanMemberRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new BanMemberCommand(userId, GroupId, request.UserId));
		return NoContent();
	}
}