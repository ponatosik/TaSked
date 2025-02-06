namespace TaSked.Domain;

public class Announcement
{
	public Guid Id { get; init; }
	public string Title { get; private set; } = null!;
	public string Message { get; set; } = null!;

	private Announcement() { }

	internal Announcement(Guid id, string title, string message)
	{
		Id = id;
		Title = title;
		Message = message;
	}
}
