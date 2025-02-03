namespace TaSked.Domain;

public record Teacher
{
	public string FullName { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public RelatedLink? OnlineMeetingUrl { get; set; }

	private Teacher() { }

	private Teacher(
		string fullName,
		string? description = null,
		string? email = null,
		string? phoneNumber = null,
		RelatedLink? onlineMeetingUrl = null)
	{
		FullName = fullName;
		Description = description;
		Email = email;
		PhoneNumber = phoneNumber;
		OnlineMeetingUrl = onlineMeetingUrl;
	}

	public static Teacher Create(
		string fullName,
		string? description = null,
		string? email = null,
		string? phoneNumber = null,
		RelatedLink? onlineLessonUrl = null)
	{
		return new Teacher(fullName, description, email, phoneNumber, onlineLessonUrl);	
	}
}
