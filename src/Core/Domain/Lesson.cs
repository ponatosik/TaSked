namespace TaSked.Domain;

public record Lesson
{
	public int SubjectId { get; private set; }
	public DateTime Time { get; private set; }
}
