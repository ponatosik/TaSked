using Microsoft.EntityFrameworkCore;
using TaSked.Infrastructure.Persistance;

namespace Application.Tests;

// A class to share context between tests.
// About Xunit Fixtures: https://xunit.net/docs/shared-context#collection-fixture
public class PersistanceFixture : IDisposable
{
	private ApplicationDbContext _dbContext;
	public ApplicationDbContext GetDbContext()
	{
		_dbContext.Database.EnsureCreated();
		return _dbContext;
	}

	public PersistanceFixture()
	{
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseSqlite("DataSource=file::memory:?cache=shared")
			.Options;

		_dbContext = new ApplicationDbContext(options);
	}

	public void Dispose()
	{
		_dbContext.Dispose();
	}
}

[CollectionDefinition("Persistance tests")]
public class PersistanceTestsCollection : ICollectionFixture<PersistanceFixture>;
