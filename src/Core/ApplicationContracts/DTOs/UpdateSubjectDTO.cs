using TaSked.Domain;

namespace TaSked.Application;

public class UpdateSubjectDTO
{
	public Guid Id { get; private set; }
	public Guid GroupId { get; private set; }
	public string Name { get; set; }
	public Teacher? Teacher { get; set; }
	public List<RelatedLink> RelatedLinks { get; set; }

	public UpdateSubjectDTO(Guid id, Guid groupId, string name, Teacher? teacher, List<RelatedLink> relatedLinks)
	{
		Id = id;
		GroupId = groupId;
		Name = name;
		Teacher = teacher;
		RelatedLinks = relatedLinks;
	}

	public static UpdateSubjectDTO From(Subject subject)
	{
		return new UpdateSubjectDTO(subject.Id, subject.GroupId, subject.Name, subject.Teacher, subject.RelatedLinks);
	}
}
