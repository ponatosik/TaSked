namespace TaSked.Domain;

public class Group
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public List<Subject> Subjects { get; private set; } = new List<Subject>();
	public List<User> Members { get; private set; } = new List<User>();
	public List<Report> Reports { get; private set; } = new List<Report>();
	public List<Invinatation> Invations { get; private set;} = new List<Invinatation>();

	private Group() { }
	private Group(Guid id, string name, User admin)
	{
		Id = id;
		Name = name;
		admin.GroupId = id;
		admin.Role = GroupRole.Admin;
	} 
	public static Group Create(string name, User creator)
	{
		if (creator.GroupId is not null)
		{
			throw new Exception("Cannot create group with admin that already in a group");
		}
		return new Group(Guid.NewGuid(),name, creator);
	}
	public Subject CreateSubject(string name)
	{
		Subject subject = Subject.Create(this.Id, name);
		Subjects.Add(subject);
		return subject;
	}
}
