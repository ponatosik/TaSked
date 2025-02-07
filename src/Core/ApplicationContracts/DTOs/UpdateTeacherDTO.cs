namespace TaSked.Domain;

public class UpdateTeacherDTO
{
	public string FullName { get; set; }
	public string? Description { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public RelatedLink? OnlineMeetingUrl { get; set; }

	public UpdateTeacherDTO(string fullName, string? description, string? email, string? phoneNumber,
		RelatedLink? onlineMeetingUrl)
	{
		FullName = fullName;
		Description = description;
		Email = email;
		PhoneNumber = phoneNumber;
		OnlineMeetingUrl = onlineMeetingUrl;
	}

	public static UpdateTeacherDTO From(Teacher teacher)
	{
		return new UpdateTeacherDTO(
			teacher.FullName,
			teacher.Description,
			teacher.Email,
			teacher.PhoneNumber,
			teacher.OnlineMeetingUrl);
	}
}