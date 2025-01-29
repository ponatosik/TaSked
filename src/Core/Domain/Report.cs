namespace TaSked.Domain;

public class Report
{
	public Guid Id { get; init; }
	public string Title { get; private set; } = null!;
	public string Message { get; set; } = null!;

	private Report() { }

	internal Report(Guid id, string title, string message)
	{
		Id = id;
		Title = title;
		Message = message;
	}
}
