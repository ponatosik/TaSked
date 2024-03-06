namespace TaSked.Domain;

public class HomeworkTask
{
	public Guid HomeworkId { get; private set; }
	public bool Completed { get; set; } = false;
	private HomeworkTask() { }
	public HomeworkTask(Guid homeworkId)
	{
		HomeworkId = homeworkId;
	}
}
