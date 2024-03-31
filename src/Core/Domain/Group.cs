using Domain.Exceptions;

namespace TaSked.Domain;

public class Group
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public List<Subject> Subjects { get; private set; } = new List<Subject>();
	public List<User> Members { get; private set; } = new List<User>();
	public List<Report> Reports { get; private set; } = new List<Report>();
	public List<Invitation> Invitations { get; private set;} = new List<Invitation>();

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
			throw new UserAlreadyInGroupException(creator);
		}
		return new Group(Guid.NewGuid(),name, creator);
	}

	public Subject CreateSubject(string name, Teacher? teacher = null)
	{
		Subject subject = Subject.Create(this.Id, name, teacher);
		Subjects.Add(subject);
		return subject;
	}

	public Invitation CreateInvintation(string? caption = null, int? maxActivations = null, DateTime? expirationDate = null)
	{
		Invitation invitation = new Invitation(Guid.NewGuid(), Id, caption, maxActivations, expirationDate);
		Invitations.Add(invitation);
		return invitation;
	}

	public void JoinByInvintation(Invitation invitation, User user)
	{
		if (invitation.IsExpired)
		{
			throw new InvitationExpiredException(invitation);
		}

		invitation.ActivateOne();
		user.JoinGroup(this);
	}

	public Report CreateReport(string Title, string Message)
	{
		Report report = new Report(Guid.NewGuid(), Title, Message);
		Reports.Add(report);
		return report;
	}

	public void Leave(User user)
	{
		user.LeaveGroup(this);
	}
}
