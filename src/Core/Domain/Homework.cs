namespace TaSked.Domain;

public class Homework
{
	public Guid Id { get; init; }
	public Guid SubjectId { get; private set; }
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public DateTime? Deadline { get; set; }
	public DateTime CreatedAt { get; private set; }
	public string? SourceUrl {  get; set; }

	private Homework() { }

	private Homework(Guid id, string title, string description)
	{
		Id = id;
		Title = title;
		Description = description;
	}
	
	
	internal Homework(Guid id, Guid subjectId, string title, string description, DateTime createdAt, DateTime? deadline = null, string? url = null)
		: this(id, title, description)
	{
		SubjectId = subjectId;
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
