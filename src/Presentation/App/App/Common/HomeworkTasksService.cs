using TaSked.Api.ApiClient;
using TaSked.Domain;
using TaSked.Infrastructure.LocalPersistence;

namespace TaSked.App.Common;

public class HomeworkTasksService
{
	private TasksDatabase _database;
	private ITaSkedHomeworks _homeworkService;

	public HomeworkTasksService(TasksDatabase database, ITaSkedHomeworks homeworkService)
	{
		_database = database;
		_homeworkService = homeworkService;
	}

	public async Task<List<HomeworkTask>> GetAllAsync()
	{
		var homeworks = (await _homeworkService.GetAllHomework()).ToList();
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
                task.MetaData = taskDAO.MetaData;
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
		task.Completed = false;
		HomeworkTaskDAO taskDAO = (await _database.GetItemAsync(task.Homework.Id)) ?? new HomeworkTaskDAO(task);
		taskDAO.Completed = false;
        await _database.SaveItemAsync(taskDAO);
	}

    public async Task UpdateStrokeColorAsync(HomeworkTask task)
    {
        HomeworkTaskDAO taskDAO = (await _database.GetItemAsync(task.Homework.Id)) ?? new HomeworkTaskDAO(task);
        taskDAO.MetaData = task.MetaData;
        await _database.SaveItemAsync(taskDAO);
    }
}
