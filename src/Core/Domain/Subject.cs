namespace TaSked.Domain;

public class Subject
{
	public Guid Id { get; init; }
	public Guid GroupId { get; private set; }
	public string Name { get; set; } = null!;
	public List<Homework> Homeworks { get; init; } = [];
	public List<Lesson> Lessons { get; init; } = [];
	public Teacher? Teacher { get; set; }

	private Subject() { }

	private Subject(Guid id, string name)
	{
		Id = id;
		Name = name;
	}

	internal Subject(Guid id, Guid groupId, string name, Teacher? teacher = null) : this(id, name)
	{
		Id = id;
		GroupId = groupId;
		Name = name;
		Teacher = teacher;
	}

	internal static Subject Create(Guid groupId, string name, Teacher? teacher = null)
	{
		return new Subject(Guid.NewGuid(), groupId, name, teacher);
	}

    public Homework CreateHomework(string title, string description, DateTime? deadline = null)
	{
		Homework homework = Homework.Create(this, title, description, deadline);
		Homeworks.Add(homework);
		return homework;
	}

	public Lesson CreateLesson(DateTime dateTime)
	{
		Lesson lesson = new Lesson(Guid.NewGuid(), Id, dateTime);
		Lessons.Add(lesson);
		return lesson;
	}
}
