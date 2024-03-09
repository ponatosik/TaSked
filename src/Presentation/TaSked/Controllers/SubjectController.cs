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

public class SubjectController : ControllerBase
{
	private readonly IMediator _mediator;

	public SubjectController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[HttpPost]
	[Route("Create")] 
	[Authorize(AccessPolicise.Moderator)]
	public async Task<IActionResult> Post([FromBody] string subjectName)
	{
		Guid userId = User.GetUserId()!.Value;
		return Ok(await _mediator.Send(new CreateSubjectCommand(userId, subjectName)));
	}
}
