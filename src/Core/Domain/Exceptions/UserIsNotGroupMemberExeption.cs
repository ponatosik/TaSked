using TaSked.Domain;

namespace Domain.Exceptions;

public class UserIsNotGroupMemberException : DomainException
{
	public string UserNickname { get; private set; }
	public Guid UserId { get; private set; }
	public Guid GroupId { get; private set; }

	private static string GenerateMessage(User user, Group group) =>
		$"User \"{user.Nickname}\" is not part of group \"{group.Name}\".";

	internal UserIsNotGroupMemberException(User user, Group group)
		: base(GenerateMessage(user, group))
	{
		UserId = user.Id;
		UserNickname = user.Nickname;
		GroupId = group.Id;
	}
	internal UserIsNotGroupMemberException(User user, Group group, Exception inner)
		: base(GenerateMessage(user, group), inner)
	{
		UserId = user.Id;
		UserNickname = user.Nickname;
		GroupId = user.GroupId!.Value;
	}
}
