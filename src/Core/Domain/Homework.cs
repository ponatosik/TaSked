namespace TaSked.Domain;

public class Homework
{
	public Guid Id { get; private set; }
	public Guid SubjectId {  get; private set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public DateTime? Deadline { get; set; }
	public DateTime CreatedAt { get; private set; }
	public string? SourceUrl {  get; set; }

	private Homework() { }
	internal Homework(Guid id, Guid subjectId, string title, string description, DateTime createdAt, DateTime? deadline = null, string? url = null)
	{
		Id = id;
		SubjectId = subjectId;
		Title = title;
		Description = description;
		CreatedAt = createdAt;
		Deadline = deadline;
		SourceUrl = url;
	}

	internal static Homework Create(Subject subject, string title, string description, DateTime? deadline = null, string? url = null)
	{
		return new Homework(Guid.NewGuid(), subject.Id, title, description, DateTime.UtcNow, deadline, url);
	}

	public HomeworkTask CreateTask()
	{
		var task = new HomeworkTask(this);
		return task;
	}
}
