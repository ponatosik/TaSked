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
	[Route("anonymousAccout")]
	public async Task<IActionResult> Post(CreateUserTokenRequest request)
	{
		var result = await _mediator.Send(new CreateUserTokenCommand(request.Nickname));
		return CreatedAtAction(nameof(Get), new { }, result);
	}

	[HttpGet]
	[Route("{UserId:guid}")]
	public async Task<IActionResult> Get(Guid UserId)
	{
		var result = await _mediator.Send(new GetUserInfoQuery(UserId));
		return Ok(result);
	}

	[HttpGet]
	[Authorize]
	[Route("account")]
	public async Task<IActionResult> Get()
	{
		Guid UserId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetUserInfoQuery(UserId));
		return Ok(result);
	}
}