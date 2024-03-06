using TaSked.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using Microsoft.AspNetCore.Authorization;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
	private readonly IMediator _mediator;

	public GroupController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	public async Task<Group> Post([FromBody] string groupName)
	{
		Guid userId = User.GetUserId()!.Value;
		return await _mediator.Send(new CreateGroupCommand(userId, groupName));
	}
}
