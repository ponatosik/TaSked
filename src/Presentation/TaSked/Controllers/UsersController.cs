using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Api.Requests;
using TaSked.Application;

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

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserRequest request)
    {
        return Ok(await _mediator.Send(new CreateUserCommand(request.Nickname)));
    }
}