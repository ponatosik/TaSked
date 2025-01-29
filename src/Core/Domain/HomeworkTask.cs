namespace TaSked.Domain;

public class HomeworkTask
{
	public Homework Homework { get; private set; } = null!;
	public bool Completed { get; set; }
	public string? MetaData { get; set; }

	private HomeworkTask() { }

	public HomeworkTask(Homework homework)
	{
		Homework = homework;
    }
}
