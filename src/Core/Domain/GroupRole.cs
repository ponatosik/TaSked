namespace TaSked.Domain;

public record GroupRole
{
	public string RoleName { get; init; } = "no group";
	public byte AccessLevel { get; init; }

	private GroupRole() { }
	
	private GroupRole(string roleName, byte accessLevel) : this()
	{
		RoleName = roleName;
		AccessLevel = accessLevel;
	}

	public static GroupRole Admin => new("admin", 3);
	public static GroupRole Moderator => new("moderator", 2);
	public static GroupRole Member => new("member", 1);
	public static GroupRole NoGroup => new("no group", 0);

	public static bool operator < (GroupRole a, GroupRole b) => a.AccessLevel < b.AccessLevel;
	public static bool operator > (GroupRole a, GroupRole b) => a.AccessLevel > b.AccessLevel;
}
