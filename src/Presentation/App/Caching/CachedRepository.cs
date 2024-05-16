using Akavache;
using System.Reactive.Linq;

namespace TaSked.App.Caching; 

public abstract class CachedRepository<TEntity>
{
	protected IBlobCache Cache;

	protected CachedRepository(IBlobCache cache)
	{
		Cache = cache;
	}
	
	protected abstract string GetEntityKey (TEntity entity);
	protected abstract Task<IEnumerable<TEntity>> FetchEntities();

	public void ClearCache()
	{
		Cache.InvalidateAllObjects<TEntity>();
	}

	protected async Task<IEnumerable<TEntity>> GetCachedEntities()
	{
		var entities = await Cache.GetAllObjects<TEntity>();
		return entities;
	}

	protected async Task<TEntity?> GetCachedEntityAsync(string cacheKey)
	{
		var entity = await Cache.GetObject<TEntity>(cacheKey);
		return entity;
	}

	protected async Task CacheEntitiesAsync(IEnumerable<TEntity> entities)
	{
		await Cache.InsertAllObjects<TEntity>(entities.ToDictionary(x => GetEntityKey(x), x => x));
	}

	protected async Task CacheEntityAsync(TEntity entity)
	{
		await Cache.InsertObject<TEntity>(GetEntityKey(entity), entity);
	}

	protected async Task<IEnumerable<TEntity>> FetchAndCacheEntities()
	{
		var entities = await FetchEntities();
		await CacheEntitiesAsync(entities);
		return entities;
	}

	protected async Task InvalidateEntity(TEntity entity)
	{
		await Cache.InvalidateObject<TEntity>(GetEntityKey(entity));
	}

	protected async Task InvalidateEntityByKey(string key)
	{
		await Cache.InvalidateObject<TEntity>(key);
	}

	protected async Task UpdateEntity(TEntity entity)
	{
		await Cache.InsertObject<TEntity>(GetEntityKey(entity), entity);
	}
} 