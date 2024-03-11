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
		return Ok(await _mediator.Send(new CreateUserTokenCommand(request.Nickname)));
	}

	[HttpGet]
	[Route("{UserId:guid}")]
	public async Task<IActionResult> Get(Guid UserId)
	{
		return Ok(await _mediator.Send(new GetUserInfoQuery(UserId)));
	}

	[HttpGet]
	[Authorize]
	[Route("account")]
	public async Task<IActionResult> Get()
	{
		Guid UserId = User.GetUserId()!.Value;
		return Ok(await _mediator.Send(new GetUserInfoQuery(UserId)));
	}

    //[HttpPost]
    //public async Task<IActionResult> Post(CreateUserRequest request)
    //{
    //    return Ok(await _mediator.Send(new CreateUserCommand(request.Nickname)));
    //}
}