using TaSked.Domain;

namespace Domain.Exceptions;

public class UserAllreadyPromotedException : DomainException
{
	public string UserNickname { get; private set; }
	public Guid UserId { get; private set; }
	public Guid GroupId { get; private set; }
	public GroupRole Role { get; private set; }

	private static string GenerateMessage(User user, Group group, GroupRole role) =>
		$"User \"{user.Nickname}\" is already a {role.RoleName} or higher in group \"{group.Name}\".";

	internal UserAllreadyPromotedException(User user, Group group, GroupRole role)
		: base(GenerateMessage(user, group, role))
	{
		UserId = user.Id;
		UserNickname = user.Nickname;
		GroupId = group.Id;
		Role = role;
	}
	internal UserAllreadyPromotedException(User user, Group group, GroupRole role, Exception inner)
		: base(GenerateMessage(user, group, role), inner)
	{
		UserId = user.Id;
		UserNickname = user.Nickname;
		GroupId = user.GroupId!.Value;
		Role = role;
	}
}
