using TaSked.Domain;

namespace TaSked.Application;

public class UpdateSubjectDTO
{
	public Guid Id { get; private set; }
	public Guid GroupId { get; private set; }
	public string Name { get; set; }
	public Teacher? Teacher { get; set; }

	public UpdateSubjectDTO(Guid Id, Guid GroupId, string Name, Teacher? Teacher)
	{
		this.Id = Id;
		this.GroupId = GroupId;
		this.Name = Name;
		this.Teacher = Teacher;
	}

	public static UpdateSubjectDTO From(Subject subject)
	{
		return new UpdateSubjectDTO(subject.Id, subject.GroupId, subject.Name, subject.Teacher);
	}
}
