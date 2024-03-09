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
public class GroupController : ControllerBase
{
	private readonly IMediator _mediator;

	public GroupController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Post(CreateGroupRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		return Ok(await _mediator.Send(new CreateGroupCommand(userId, request.GroupName)));
	}
}
