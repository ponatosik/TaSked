using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaSked.Application.Exceptions;
using ApplicationException = TaSked.Application.Exceptions.ApplicationException;

namespace TaSked.Infrastructure.ExceptionHandling;

public class ApplicationExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public ApplicationExceptionHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next.Invoke(context);
		}
		catch (ApplicationException exception)
		{
			var problemDetail = GetDetails(exception);
			problemDetail.Instance = context.Request.Path;
		
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = problemDetail.Status ?? 500;

			await context.Response.WriteAsJsonAsync(problemDetail);
		}
	}

	private ProblemDetails GetDetails(ApplicationException exception)
	{
		return exception switch
		{
			EntityNotFoundException => new ProblemDetails()
			{
				Title = exception.Message,
				Detail = "Update application or try again later.",
				Status = StatusCodes.Status404NotFound,
			},	
			UserIsNotGroupMemberException => new ProblemDetails()
			{
				Title = exception.Message,
				Detail = "You are not a member of this group.",
				Status = StatusCodes.Status403Forbidden
			},
			UserNicknameAlreadyTaken => new ProblemDetails
			{
				Title = exception.Message,
				Detail = "User with given nickname already exists.",
				Status = StatusCodes.Status409Conflict
			},
			ApplicationException => new ProblemDetails()
			{
				Title = exception.Message,
				Status = StatusCodes.Status500InternalServerError
			}
		};
	}
}
