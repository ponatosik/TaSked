using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;

namespace TaSked.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	[Route("anonymousAccout")]
	public async Task<string> Post(CreateUserTokenCommand command)
	{
		return await _mediator.Send(command);
	}

}
