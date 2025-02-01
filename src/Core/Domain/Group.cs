using Domain.Exceptions;

namespace TaSked.Domain;

public class Group
{
	public Guid Id { get; init; }
	public string Name { get; set; } = null!;
	public List<Subject> Subjects { get; } = [];
	public List<User> Members { get; private set; } = [];
	public List<Report> Reports { get; } = [];
	public List<Invitation> Invitations { get; } = [];

	private Group() { }

	private Group(Guid id, string name)
	{
		Id = id;
		Name = name;
	}

	private Group(Guid id, string name, User admin) : this(id, name)
	{
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
		var subject = Subject.Create(Id, name, teacher);
		Subjects.Add(subject);
		return subject;
	}

	public Invitation CreateInvitation(string? caption = null, int? maxActivations = null,
		DateTime? expirationDate = null)
	{
		Invitation invitation = new Invitation(Guid.NewGuid(), Id, caption, maxActivations, expirationDate);
		Invitations.Add(invitation);
		return invitation;
	}

	public void JoinByInvitation(Invitation invitation, User user)
	{
		if (invitation.IsExpired)
		{
			throw new InvitationExpiredException(invitation);
		}

		invitation.ActivateOne();
		user.JoinGroup(this);
	}

	public Report CreateReport(string title, string message)
	{
		var report = new Report(Guid.NewGuid(), title, message);
		Reports.Add(report);
		return report;
	}

	public void Leave(User user)
	{
		user.LeaveGroup(this);
	}
}
