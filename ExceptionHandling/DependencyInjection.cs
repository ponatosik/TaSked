using Microsoft.AspNetCore.Builder;

namespace TaSked.Infrastructure.ExceptionHandling;

public static class DependencyInjection
{
	public static IApplicationBuilder UseDomainExceptionHandling(this IApplicationBuilder builder)
	{
		builder.UseMiddleware<DomainExceptionHandlingMiddleware>();
		return builder;
	}
}
