namespace TaSked.Domain;

public class Comment
{
	public Guid Id { get; init; }
	public User Author { get; init; } = null!;
	public string Content { get; set; } = null!;

	private Comment() { }

	private Comment(Guid id, User user, string content)
	{
		Id = id;
		Author = user;
		Content = content;
	}

	internal static Comment Create(User user, string content)
	{
		return new Comment(Guid.NewGuid(), user, content);
	}
}