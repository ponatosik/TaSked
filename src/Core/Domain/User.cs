using Domain.Exceptions;

namespace TaSked.Domain;

public class User
{
	public Guid Id { get; init; }
	public Guid? GroupId { get; set; }
	public string Nickname { get; set; } = null!;
	public GroupRole Role { get; internal set; } = GroupRole.NoGroup;

	private User() { }

	private User(Guid id, string nickname)
	{
		Id = id;
		Nickname = nickname;
	}

	public static User Create(string nickname)
	{
		return new User(Guid.NewGuid(), nickname);
	}

	public void JoinGroup(Group group)
	{
		if (GroupId is not null) 
		{
			throw new UserAlreadyInGroupException(this);
		}

		GroupId = group.Id;
		group.Members.Add(this);
		Role = GroupRole.Member;
	}

	public void Promote(GroupRole role, Group group)
	{
		if (Role > role)
		{
			throw new UserAlreadyPromotedException(this, group, role);
		}
		if (GroupId is null || role == GroupRole.NoGroup)
		{
			throw new UserIsNotGroupMemberException(this, group);
		}
		Role = role;
	}

	public void Demote(GroupRole role, Group group)
	{
		if (Role < role)
		{
			throw new UserAlreadyDemotedException(this, group, role);
		}
		if (GroupId is null || role == GroupRole.NoGroup)
		{
			throw new UserIsNotGroupMemberException(this, group);
		}
		Role = role;
	}

	public void LeaveGroup(Group group)
	{
		if (GroupId != group.Id)
		{
			throw new UserIsNotGroupMemberException(this, group);
		}

		group.Members.Remove(this);
		GroupId = null;
		Role = GroupRole.NoGroup;
	}
}
