namespace TaSked.Infrastructure.LocalPersistence;

public class LocalPersistenceOptions
{
	public string DatabaseFolder { get; set; }

	public LocalPersistenceOptions(string databaseFolder)
	{
		DatabaseFolder = databaseFolder;
	}
}
