namespace TaSked.Domain;

public record GroupRole
{
	public string RoleName { get; private set; } = "no group";
	public byte AccessLevel { get; private set; } = 0;

	private GroupRole(string roleName, byte accessLevel) 
	{
		RoleName = roleName;
		AccessLevel = accessLevel;
	}
	private GroupRole() { }

	public static GroupRole Admin => new("admin", 3);
	public static GroupRole Moderator => new("moderator", 2);
	public static GroupRole Member => new("member", 1);
	public static GroupRole NoGroup => new("no group", 0);
}
