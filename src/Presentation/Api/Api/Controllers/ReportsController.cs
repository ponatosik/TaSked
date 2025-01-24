using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaSked.Api.Requests;
using TaSked.Application;
using TaSked.Infrastructure.Authorization;

namespace TaSked.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AccessPolicies.Member)]
public class ReportsController : ControllerBase
{
	private readonly IMediator _mediator;

	public ReportsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[Authorize(AccessPolicies.Moderator)]
	public async Task<IActionResult> Post(CreateReportRequest request)
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new CreateReportCommand(userId, request.ReportTitle, request.ReportMessage));
		return CreatedAtAction(nameof(Get), new { }, result);
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		Guid userId = User.GetUserId()!.Value;
		var result = await _mediator.Send(new GetAllReportQuery(userId));
		return Ok(result);
	}
}