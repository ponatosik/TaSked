using TaSked.Domain;

namespace TaSked.Application;

public class SubjectDTO
{
	public Guid Id { get; private set; }
	public Guid GroupId { get; private set; }
	public string Name { get; set; }
	public int HomeworksCount { get; set; }
	public int LessonsCount { get; set; }
	public List<Teacher> Teachers { get; set; }
	public List<RelatedLink> RelatedLinks { get; set; }

	public SubjectDTO(Guid id, Guid groupId, string name, int homeworksCount, int lessonsCount, List<Teacher> teachers,
		List<RelatedLink> relatedLinks)
	{
		Id = id;
		GroupId = groupId;
		Name = name;
		HomeworksCount = homeworksCount;
		LessonsCount = lessonsCount;
		Teachers = teachers;
		RelatedLinks = relatedLinks;
	}

	public static SubjectDTO From(Subject subject)
	{
		int homeworksCount = subject.Homeworks.Count;
		int lessonsCount = subject.Lessons.Count;
		return new SubjectDTO(
			subject.Id,
			subject.GroupId,
			subject.Name,
			homeworksCount,
			lessonsCount,
			subject.Teachers,
			subject.RelatedLinks);
	}
}
