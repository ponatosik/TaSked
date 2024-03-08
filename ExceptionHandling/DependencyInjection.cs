using Microsoft.AspNetCore.Builder;

namespace TaSked.Infrastructure.ExceptionHandling;

public static class DependencyInjection
{
	public static IApplicationBuilder UseDomainExceptionHandling(this IApplicationBuilder builder)
	{
		builder.UseMiddleware<DomainExceptionHandlingMiddleware>();
		return builder;
	}

	public static IApplicationBuilder UseApplicationExceptionHandling(this IApplicationBuilder builder)
	{
		builder.UseMiddleware<ApplicationExceptionHandlingMiddleware>();
		return builder;
	}
}
