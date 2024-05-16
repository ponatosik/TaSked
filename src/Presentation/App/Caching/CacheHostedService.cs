using Akavache;
using Akavache.Sqlite3;
using Microsoft.Extensions.Hosting;

namespace TaSked.App.Caching;

internal class CacheHostedService : IHostedService
{
	static CacheHostedService()
	{
		Akavache.Registrations.Start("TaSked");
	}

	public Task StartAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		 await BlobCache.Shutdown();
	}

	public static IBlobCache GetCache()
	{
		return BlobCache.LocalMachine;
	}
}


// https://github.com/reactiveui/Akavache?tab=readme-ov-file#handling-xamarinmaui-linker
public static class LinkerPreserve
{
	static LinkerPreserve()
	{
		var persistentName = typeof(SQLitePersistentBlobCache).FullName;
		var encryptedName = typeof(SQLiteEncryptedBlobCache).FullName;
	}
}

