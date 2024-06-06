using SQLite;
using System.Drawing;
using System.Threading.Tasks;
using TaSked.Domain;

namespace TaSked.Infrastructure.LocalPersistence;

public class HomeworkTaskDAO
{
	[PrimaryKey, AutoIncrement]
	public int? ID { get; set; }
	public Guid HomeworkId { get; set; }
	public Guid SubjectId { get; set; }
	public bool Completed { get; set; }
    public string? MetaData { get; set; }

    public HomeworkTaskDAO() { }
	public HomeworkTaskDAO(HomeworkTask homeworkTask)
	{
		HomeworkId = homeworkTask.Homework.Id;
		SubjectId = homeworkTask.Homework.SubjectId;
		Completed = homeworkTask.Completed;
        MetaData = homeworkTask.MetaData;
    }
}
