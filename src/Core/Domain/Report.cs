namespace TaSked.Domain;

public class Report
{
	public Guid Id { get; init; }
	public string Title { get; private set; }
	public string Message { get; set; }

	internal Report(Guid id, string title, string message)
	{
		Id = id;
		Title = title;
		Message = message;
	}
}
