using SQLite;

namespace TaSked.Infrastructure.LocalPersistence;

public class TasksDatabase
{
	private SQLiteAsyncConnection _database;
	private readonly string _databaseFolder;


	public TasksDatabase(string databaseFolder)
	{
		_databaseFolder = databaseFolder;
	}

	async Task Init()
	{
		if (_database is not null)
		{
			return;
		}

		_database = new SQLiteAsyncConnection(Constants.DatabasePath(_databaseFolder), Constants.Flags);
		var result = await _database.CreateTableAsync<HomeworkTaskDAO>();
	}

	public async Task<List<HomeworkTaskDAO>> GetTasksAsync()
	{
		await Init();
		return await _database.Table<HomeworkTaskDAO>().ToListAsync();
	}

	public async Task<List<HomeworkTaskDAO>> GetTasksNotCompletedAsync()
	{
		await Init();
		return await _database.Table<HomeworkTaskDAO>().Where(t => t.Completed).ToListAsync();
	}

	public async Task<HomeworkTaskDAO?> GetItemAsync(Guid homeworkId)
	{
		await Init();
		return await _database.Table<HomeworkTaskDAO>().Where(t => t.HomeworkId == homeworkId).FirstOrDefaultAsync();
	}

	public async Task SaveItemAsync(HomeworkTaskDAO item)
	{
		await Init();
		if (item.Id != 0)
		{
			await _database.UpdateAsync(item);
		}
		else
		{
			await _database.InsertAsync(item);
		}
	}

	public async Task<int> DeleteItemAsync(HomeworkTaskDAO item)
	{
		await Init();
		return await _database.DeleteAsync(item);
	}

	public async Task Clear()
	{
		await Init();
		await _database.DeleteAllAsync<HomeworkTaskDAO>();
	}
}