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
public class ReportsController : ControllerBase
{
	private readonly IMediator _mediator;

	public ReportsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[Authorize(AccessPolicise.Moderator)]
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