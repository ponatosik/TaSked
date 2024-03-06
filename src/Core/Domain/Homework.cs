namespace TaSked.Domain;

public class Homework
{
	public Guid Id { get; private set; }
	public Guid SubjectId {  get; private set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public DateTime? Deadline { get; private set; }
	public DateTime CreatedAt { get; private set; }
	public string? SourceUrl {  get; set; }
	private Homework() { }
	internal Homework(Guid id, Guid subjectId, string title, string description, DateTime createdAt)
	{
		Id = id;
		SubjectId = subjectId;
		Title = title;
		Description = description;
		CreatedAt = createdAt;
	}

	internal static Homework Create(Subject subject, string title, string description)
	{
		return new Homework(Guid.NewGuid(), subject.Id, title, description, DateTime.UtcNow);
	}

	public HomeworkTask CreateTask()
	{
		var task = new HomeworkTask(this.Id);
		return task;
	}
}
