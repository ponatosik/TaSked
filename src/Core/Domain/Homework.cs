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
	public List<Comment> Comments { get; init; } = [];
	public string? BriefSummary { get; set; }

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
		IEnumerable<RelatedLink>? relatedLinks = null,
		string? briefSummary = null)
		: this(id, title, description)
	{
		SubjectId = subjectId;
		CreatedAt = createdAt.ToUniversalTime();
		Deadline = deadline?.ToUniversalTime();
		RelatedLinks = (relatedLinks ?? []).ToList();
		BriefSummary = briefSummary;	
	}

	internal static Homework Create(Subject subject, string title, string description, DateTime? deadline = null,
		List<RelatedLink>? relatedLinks = null, string? briefSummary = null)
	{
		return new Homework(Guid.NewGuid(), subject.Id, title, description, DateTime.UtcNow, deadline, relatedLinks,
			briefSummary);
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

	public Comment LeaveComment(User user, string content)
	{
		var comment = Comment.Create(user, content);
		Comments.Add(comment);
		return comment;
	}
}
