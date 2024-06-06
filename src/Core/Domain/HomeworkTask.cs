using System.Drawing;

namespace TaSked.Domain;

public class HomeworkTask
{
	public Homework Homework { get; private set; } 
	public bool Completed { get; set; } = false;
	public string? MetaData { get; set; }

    private HomeworkTask() { }
	public HomeworkTask(Homework homework)
	{
		Homework = homework;
    }
}
