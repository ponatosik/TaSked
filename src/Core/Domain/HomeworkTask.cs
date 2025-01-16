namespace TaSked.Domain;

public class HomeworkTask
{
	public Homework Homework { get; private set; }
	public bool Completed { get; set; }
	public string? MetaData { get; set; }

	public HomeworkTask(Homework homework)
	{
		Homework = homework;
    }
}
