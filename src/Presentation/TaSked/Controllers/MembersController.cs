using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using Microsoft.AspNetCore.Authorization;
using TaSked.Infrastructure.Authorization;
using TaSked.Api.Requests;

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
	[Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Get(Guid GroupId)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetGroupMembersQuery(userId)));
    }
}