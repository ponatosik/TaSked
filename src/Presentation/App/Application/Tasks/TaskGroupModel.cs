namespace TaSked.App;

public class TaskGroupModel
{
	public string Title { get; set; }
	public List<TaskViewModel> Tasks { get; set; }

    public TaskGroupModel(string title, List<TaskViewModel> tasks)
    {
        Title = title;
        Tasks = tasks;
    }
}