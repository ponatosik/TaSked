namespace TaSked.Application.Exceptions;

public class UserNicknameAlreadyTaken : ApplicationException
{
	public string Nickname { get; private set; }

	private static string GenerateMessage(string nickname)
	{
		return $"User nickname already taken: {nickname}.";
	}

	internal UserNicknameAlreadyTaken(string nickname) : base(GenerateMessage(nickname))
	{
		Nickname = nickname;
	}

	internal UserNicknameAlreadyTaken(string nickname, Exception inner) : base(GenerateMessage(nickname), inner)
	{
		Nickname = nickname;
	}
}