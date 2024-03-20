namespace TaSked.Infrastructure.LocalPersistence;

public class HomeworkTaskDAO
{
	public int Id { get; set; }
	public Guid HomeworkId { get; set; }
	public Guid SubjectId { get; set; }
	public bool Completed { get; set; }
}
