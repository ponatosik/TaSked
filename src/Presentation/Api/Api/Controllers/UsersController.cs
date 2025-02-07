using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	private readonly IMediator _mediator;

	public UsersController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	[Route("Anonymous")]
	public async Task<IActionResult> Post(CreateAnonymousUserTokenRequest request)
	{
		var result = await _mediator.Send(new CreateUserTokenCommand(request.Nickname));
		return CreatedAtAction(nameof(Get), new { }, result);
	}

	[HttpGet]
	[Route("{userId:guid}")]
	public async Task<IActionResult> Get(Guid userId)
	{
		var result = await _mediator.Send(new GetUserInfoQuery(userId));
		return Ok(result);
	}

	[HttpGet]
	[Authorize]
	[Route("Account")]
	public async Task<IActionResult> Get()
	{
		var userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetUserInfoQuery(userId));
		return Ok(result);
	}
}