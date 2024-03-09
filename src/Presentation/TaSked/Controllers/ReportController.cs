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
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(AccessPolicise.Moderator)]
    public async Task<IActionResult> Post(CreateReportRequest request)
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new CreateReportCommand(userId, request.ReportTitle, request.ReportMessage)));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        Guid userId = User.GetUserId()!.Value;
        return Ok(await _mediator.Send(new GetAllReportQuery(userId)));
    }
}