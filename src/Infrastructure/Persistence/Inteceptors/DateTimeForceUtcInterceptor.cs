using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaSked.Infrastructure.Persistence.Inteceptors;

public class DateTimeForceUtcInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		if (eventData.Context is null)
		{
			return await base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		foreach (var entry in eventData.Context.ChangeTracker.Entries())
		{
			EnsureUtc(entry);
		}

		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void EnsureUtc(EntityEntry entry)
	{
		if (entry.State != EntityState.Added && entry.State != EntityState.Modified)
		{
			return;
		}

		foreach (var property in entry.Properties)
		{
			var propType = property.Metadata.ClrType;
			if (propType == typeof(DateTime) || propType == typeof(DateTime?))
			{
				property.CurrentValue = ConvertToUtc(property.CurrentValue);
			}
		}
	}

	private static object? ConvertToUtc(object? value)
	{
		if (value is DateTime dateTime && dateTime.Kind != DateTimeKind.Utc)
		{
			return dateTime.ToUniversalTime();
		}

		return value;
	}
}