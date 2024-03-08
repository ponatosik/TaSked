using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaSked.Infrastructure.ExceptionHandling;

public class DomainExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public DomainExceptionHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next.Invoke(context);
		}
		catch (DomainException exception)
		{
			var problemDetail = GetDetails(exception);
			problemDetail.Instance = context.Request.Path;
		
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = problemDetail.Status ?? 500;

			await context.Response.WriteAsJsonAsync(problemDetail);
		}
	}

	private ProblemDetails GetDetails(DomainException exception)
	{
		return exception switch
		{
			UserAlreadyInGroupException => new ProblemDetails()
			{
				Title = exception.Message,
				Detail = "Leave your current group to perform this action.",
				Status = StatusCodes.Status409Conflict
			},
			InvitationExpiredException => new ProblemDetails()
			{
				Title = exception.Message,
				Detail = "Use another invitation or ask group administrators to create new one.",
				Status = StatusCodes.Status410Gone
			},
			UserIsNotGroupMemberException => new ProblemDetails() 
			{
				Title = exception.Message,
				Detail = "You are not a member of this group.",
				Status = StatusCodes.Status403Forbidden
			},
			DomainException => new ProblemDetails()
			{
				Title = exception.Message,
				Status = StatusCodes.Status500InternalServerError
			}
		};
	}

}
