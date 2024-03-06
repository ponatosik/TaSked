using TaSked.Application.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaSked.Infrastructure.Persistance;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistance(this IServiceCollection services)
	{
		services.AddDbContext<ApplicationDbContext>();
		services.AddScoped<IApplicationDbContext> (sp => sp.GetRequiredService<ApplicationDbContext>());
		return services;
	}
}
