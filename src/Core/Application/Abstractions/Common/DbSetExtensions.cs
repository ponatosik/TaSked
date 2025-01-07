using Microsoft.EntityFrameworkCore;
using TaSked.Application.Exceptions;

namespace TaSked.Application;

internal static class DbSetExtensions
{
	public static T FindOrThrow<T> (this DbSet<T> dbSet, Guid id) where T : class 
	{
		return dbSet.Find(id) ?? throw new EntityNotFoundException(id, typeof(T).Name);
	}

	public static async Task<T> FindOrThrowAsync<T>(this DbSet<T> dbSet, Guid id, CancellationToken cancellationToken = default) 
		where T : class
	{
		return await dbSet.FindAsync([id], cancellationToken: cancellationToken) 
		       ?? throw new EntityNotFoundException(id, typeof(T).Name);
	}
}
