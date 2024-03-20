using TaSked.Domain;

namespace TaSked.Infrastructure.LocalPersistence;

public class HomeworkTaskDAO
{
	public int Id { get; set; }
	public Guid HomeworkId { get; set; }
	public Guid SubjectId { get; set; }
	public bool Completed { get; set; }

	public HomeworkTaskDAO() { }
	public HomeworkTaskDAO(HomeworkTask homeworkTask)
	{
		HomeworkId = homeworkTask.Homework.Id;
		SubjectId = homeworkTask.Homework.SubjectId;
		Completed = homeworkTask.Completed;
	}
}
