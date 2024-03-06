namespace TaSked.Domain;

public class Lesson
{
	public Guid Id { get; set; }
	public Guid SubjectId { get; private set; }
	public DateTime Time { get; private set; }

	private Lesson() { }
	internal Lesson(Guid id, Guid subjectId, DateTime time) 
	{ 
		Id = id;
		SubjectId = subjectId;
		Time = time;
	}
}
