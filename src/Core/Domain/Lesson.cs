namespace TaSked.Domain;

public class Lesson
{
	public Guid Id { get; init; }
	public Guid SubjectId { get; private set; }
	public DateTime Time { get; set; }

	private Lesson() { }

	private Lesson(Guid id)
	{
		Id = id;
	}

	internal Lesson(Guid id, Guid subjectId, DateTime time) : this(id)
	{ 
		SubjectId = subjectId;
		Time = time;
	}
}
