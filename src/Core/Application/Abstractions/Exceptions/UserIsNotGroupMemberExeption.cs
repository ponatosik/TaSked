using TaSked.Domain;

namespace TaSked.Application.Exceptions;

public class UserIsNotGroupMemberException : ApplicationException
{
	public Guid UserId { get; private set; }
	public Guid GroupId { get; private set; }

	private static string GenerateMessage(Guid userId, Guid groupId) =>
		$"User with id {userId} is not part of group with id {groupId}.";

	internal UserIsNotGroupMemberException(Guid userId, Guid groupId)
		: base(GenerateMessage(userId, groupId))
	{
		UserId = userId;
		GroupId = groupId;
	}
	internal UserIsNotGroupMemberException(Guid userId, Guid groupId, Exception inner)
		: base(GenerateMessage(userId, groupId), inner)
	{
		UserId = userId;
		GroupId = groupId;
	}
}
