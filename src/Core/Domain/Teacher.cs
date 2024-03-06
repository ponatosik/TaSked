namespace TaSked.Domain;

public record Teacher
{
	public string FullName { get; set; } = "Unspecified";
	public string? Description {  get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? OnlineLessonsUrl { get; set; }

	private Teacher() { }
}
