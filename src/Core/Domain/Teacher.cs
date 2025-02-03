namespace TaSked.Domain;

public class Teacher
{
	public Guid Id { get; init; }
	public string FullName { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public RelatedLink? OnlineMeetingUrl { get; set; }

	private Teacher() { }

	private Teacher(
		Guid id,
		string fullName,
		string? description = null,
		string? email = null,
		string? phoneNumber = null,
		RelatedLink? onlineMeetingUrl = null)
	{
		Id = id;
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
		return new Teacher(Guid.NewGuid(), fullName, description, email, phoneNumber, onlineLessonUrl);	
	}
}
