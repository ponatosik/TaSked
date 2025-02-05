namespace TaSked.Domain;

public class Subject
{
	public Guid Id { get; init; }
	public Guid GroupId { get; private set; }
	public string Name { get; set; } = null!;
	public List<Homework> Homeworks { get; init; } = [];
	public List<Lesson> Lessons { get; init; } = [];
	public List<Teacher> Teachers { get; init; } = [];
	public List<RelatedLink> RelatedLinks { get; init; } = [];
	public List<Comment> Comments { get; init; } = [];

	private Subject() { }

	private Subject(Guid id, string name)
	{
		Id = id;
		Name = name;
	}

	internal Subject(
		Guid id,
		Guid groupId,
		string name,
		List<Teacher>? teacher = null,
		IEnumerable<RelatedLink>? relatedLinks = null
	) : this(id, name)
	{
		Id = id;
		GroupId = groupId;
		Name = name;
		Teachers = teacher?.ToList() ?? [];
		RelatedLinks = (relatedLinks ?? []).ToList();
	}

	internal static Subject Create(Guid groupId, string name, List<Teacher>? teachers = null)
	{
		return new Subject(Guid.NewGuid(), groupId, name, teachers);
	}

	public Homework CreateHomework(string title, string description, DateTime? deadline = null,
		List<RelatedLink>? relatedLinks = null, string? briefSummary = null)
	{
		var homework = Homework.Create(this, title, description, deadline, relatedLinks, briefSummary);
		Homeworks.Add(homework);
		return homework;
	}

	public Lesson CreateLesson(DateTime dateTime)
	{
		Lesson lesson = new Lesson(Guid.NewGuid(), Id, dateTime);
		Lessons.Add(lesson);
		return lesson;
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
