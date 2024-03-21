using TaSked.Api.ApiClient;
using TaSked.Domain;
using TaSked.Infrastructure.LocalPersistence;

namespace TaSked.App.Common;

public class HomeworkTasksService
{
	private TasksDatabase _database;
	private ITaSkedSevice _api;

	public HomeworkTasksService(TasksDatabase database, ITaSkedSevice api)
	{
		_database = database;
		_api = api;
	}

	public async Task<List<HomeworkTask>> GetAllAsync()
	{
		var homeworks = await _api.GetAllHomework();
		var taskDAOs = await _database.GetTasksAsync();
		List<HomeworkTask> list = new List<HomeworkTask>();

		foreach (var homework in homeworks)
		{
			HomeworkTask task = homework.CreateTask();
			HomeworkTaskDAO? taskDAO = taskDAOs.FirstOrDefault(dao => dao.HomeworkId == homework.Id); 
			
			if (taskDAO == null)
			{
				await _database.SaveItemAsync(taskDAO = new HomeworkTaskDAO(task));	
			}
			else
			{
				task.Completed = taskDAO.Completed;
			}

			list.Add(task);
		}
		return list;
	}

	public async Task<List<HomeworkTask>> GetNotCompletedAsync()
	{
		return (await GetAllAsync()).Where(task => !task.Completed).ToList();
	}

	public async Task CompleteAsync(HomeworkTask task)
	{
		task.Completed = true;
		HomeworkTaskDAO taskDAO = (await _database.GetItemAsync(task.Homework.Id)) ?? new HomeworkTaskDAO(task);
		taskDAO.Completed = true;
		await _database.SaveItemAsync(taskDAO);
	}

	public async Task UndoCompletionAsync(HomeworkTask task)
	{
		task.Completed = true;
		HomeworkTaskDAO taskDAO = (await _database.GetItemAsync(task.Homework.Id)) ?? new HomeworkTaskDAO(task);
		taskDAO.Completed = false;
		await _database.SaveItemAsync(taskDAO);
	}
}
