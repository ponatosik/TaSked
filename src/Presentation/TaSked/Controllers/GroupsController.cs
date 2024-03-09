using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using Microsoft.AspNetCore.Authorization;
using TaSked.Infrastructure.Authorization;
using TaSked.Api.Requests;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GroupsController : ControllerBase
{
	private readonly IMediator _mediator;

	public GroupsController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	public async Task<IActionResult> Post(CreateGroupRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		return Ok(await _mediator.Send(new CreateGroupCommand(userId, request.GroupName)));
	}

    [HttpDelete]
	[Authorize(AccessPolicise.Admin)]
    public async Task<IActionResult> Delete()
    {
        Guid userId = User.GetUserId()!.Value;
		await _mediator.Send(new DeleteGroupCommand(userId));
        return Ok();
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Admin)]
    [Route("Name")]
    public async Task<IActionResult> Patch(ChangeGroupNameRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new ChangeGroupNameCommand(userId, request.GroupName)));
    }

    [HttpGet]
    [Route("Members")]
	[Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Get()
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetGroupMembersQuery(userId)));
    }

    [HttpPatch]
    [Authorize(AccessPolicise.Member)]
    [Route("Leave")]
    public async Task<IActionResult> Patch()
    {
        Guid userId = User.GetUserId()!.Value;
        await _mediator.Send(new LeaveGroupCommand(userId));
        return Ok();
    }
}