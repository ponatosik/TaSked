namespace TaSked.Domain;

public record GroupRole
{
	public string RoleName { get; private set; } = "no group";

	private GroupRole(string roleName) 
	{
		RoleName = roleName;
	}
	private GroupRole() { }

	public static GroupRole Admin => new("admin");
	public static GroupRole Moderator => new("moderator");
	public static GroupRole Member => new("member");
	public static GroupRole NoGroup => new("no group");
}
