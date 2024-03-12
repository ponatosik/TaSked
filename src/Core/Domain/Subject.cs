using System.Runtime.InteropServices;

namespace TaSked.Domain;

public class Subject
{
	public Guid Id { get; private set; }
	public Guid GroupId { get; private set; }
	public string Name { get; set; }
	public List<Homework> Homeworks { get; private set; } = new List<Homework>();
	public List<Lesson> Lessons { get; private set; } = new List<Lesson>();
	public Teacher Teacher { get; set; }

	private Subject() { }
	internal Subject(Guid id, Guid groupId, string name, Teacher? teacher = null)
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

    public Homework CreateHomework(string title, string description)
	{
		Homework homework = Homework.Create(this, title, description);
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
