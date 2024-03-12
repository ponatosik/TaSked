using TaSked.Domain;

namespace Domain.Exceptions;

public class UserAlreadyInGroupException : DomainException
{
	public string UserNickname { get; private set; }
	public Guid UserId { get; private set; }
	public Guid GroupId { get; private set; }

	private static string GenerateMessage(User user) => 
		$"User \"{user.Nickname}\" already has a group.";

	internal UserAlreadyInGroupException(User user) 
		: base(GenerateMessage(user))
	{
		UserId = user.Id;
		UserNickname = user.Nickname;
		GroupId = user.GroupId!.Value;
	}
	internal UserAlreadyInGroupException(User user, Exception inner)
		: base(GenerateMessage(user), inner)
	{
		UserId = user.Id;
		UserNickname = user.Nickname;
		GroupId = user.GroupId!.Value;
	}
}
