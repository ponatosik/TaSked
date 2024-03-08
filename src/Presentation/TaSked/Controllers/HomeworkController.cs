using TaSked.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application;
using Microsoft.AspNetCore.Authorization;
using TaSked.Infrastructure.Authorization;

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
	public async Task<IEnumerable<Homework>> Get()
	{
		Guid userId = User.GetUserId()!.Value;
		return await _mediator.Send(new GetAllHomeworkQuery(userId));
	}
	
	[HttpPost]
	[Authorize(AccessPolicise.Moderator)]
	public async Task<Homework> Post(CreateHomeworkCommand command)
	{
		// TODO: refactor parameters to request object
		Guid userId = User.GetUserId()!.Value;
		var newCommand = new CreateHomeworkCommand(userId, command.SubjectId, command.Title, command.Description);
		return await _mediator.Send(newCommand);
	}
}
