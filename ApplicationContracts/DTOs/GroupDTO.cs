using TaSked.Domain;

namespace TaSked.Application;

public class GroupDTO
{
	public Guid Id { get; set; }
	public string Name { get; set; }

	public GroupDTO(Guid id, string name)
	{
		Id = id;
		Name = name;
	}

	public static GroupDTO From(Group group) => new GroupDTO(group.Id, group.Name);
}
