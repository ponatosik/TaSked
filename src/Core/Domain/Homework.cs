namespace TaSked.Domain;

public class Homework
{
	public Guid Id { get; init; }
	public Guid SubjectId { get; private set; }
	public string Title { get; set; } = null!;
	public string Description { get; set; } = null!;
	public DateTime? Deadline { get; set; }
	public DateTime CreatedAt { get; private set; }
	public List<RelatedLink> RelatedLinks { get; init; } = [];

	private Homework() { }

	private Homework(Guid id, string title, string description)
	{
		Id = id;
		Title = title;
		Description = description;
	}


	internal Homework(
		Guid id,
		Guid subjectId,
		string title,
		string description,
		DateTime createdAt,
		DateTime? deadline = null,
		IEnumerable<RelatedLink>? relatedLinks = null)
		: this(id, title, description)
	{
		SubjectId = subjectId;
		CreatedAt = createdAt;
		Deadline = deadline;
		RelatedLinks = (relatedLinks ?? []).ToList();
	}

	internal static Homework Create(Subject subject, string title, string description, DateTime? deadline = null,
		List<RelatedLink>? relatedLinks = null)
	{
		return new Homework(Guid.NewGuid(), subject.Id, title, description, DateTime.UtcNow, deadline, relatedLinks);
	}

	public HomeworkTask CreateTask()
	{
		var task = new HomeworkTask(this);
		return task;
	}

	public void AddRelatedLink(RelatedLink relatedLink)
	{
		RelatedLinks.Add(relatedLink);
	}
}
