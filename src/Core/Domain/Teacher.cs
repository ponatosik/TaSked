namespace TaSked.Domain;

public record Teacher
{
	public string? FullName { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? OnlineLessonsUrl { get; set; }

	public Teacher(
		string? fullName = null,
		string? description = null,
		string? email = null,
		string? phoneNumber = null,
		string? onlineLessonsUrl = null)
	{
		FullName = fullName;
		Description = description;
		Email = email;
		PhoneNumber = phoneNumber;
		OnlineLessonsUrl = onlineLessonsUrl;
	}

	public static Teacher Create(
		string? fullName = null,
		string? description = null,
		string? email = null,
		string? phoneNumber = null,
		string? onlineLessonUrl = null)
	{
		return new Teacher(fullName, description, email, phoneNumber, onlineLessonUrl);	
	}
}
