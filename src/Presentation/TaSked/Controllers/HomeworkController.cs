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
[Authorize(AccessPolicise.Member)]
public class HomeworkController : ControllerBase
{
	private readonly IMediator _mediator;

	public HomeworkController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		Guid userId = User.GetUserId()!.Value;
		return Ok(await _mediator.Send(new GetAllHomeworkQuery(userId)));
	}
	
	[HttpPost]
	[Authorize(AccessPolicise.Moderator)]
	public async Task<IActionResult> Post(CreateHomeworkRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var newCommand = new CreateHomeworkCommand(userId, request.SubjectId, request.Title, request.Description);
		return Ok(await _mediator.Send(newCommand));
	}
}
