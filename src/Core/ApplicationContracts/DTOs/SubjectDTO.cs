using TaSked.Domain;

namespace TaSked.Application;

public class SubjectDTO
{
	public Guid Id { get; private set; }
	public Guid GroupId { get; private set; }
	public string Name { get; set; }
	public int HomeworksCount { get; set; }
	public int LessonsCount { get; set; }
	public Teacher? Teacher { get; set; }

	public SubjectDTO(Guid Id, Guid GroupId, string Name, int HomeworksCount, int LessonsCount, Teacher? Teacher)
	{
		this.Id = Id;
		this.GroupId = GroupId;
		this.Name = Name;
		this.HomeworksCount = HomeworksCount;
		this.LessonsCount = LessonsCount;	
		this.Teacher = Teacher;
	}

	public static SubjectDTO From(Subject subject)
	{
		int homeworksCount = subject.Homeworks.Count;
		int lessonsCount = subject.Lessons.Count;
		return new SubjectDTO(subject.Id, subject.GroupId, subject.Name, homeworksCount, lessonsCount, subject.Teacher);
	}
}
