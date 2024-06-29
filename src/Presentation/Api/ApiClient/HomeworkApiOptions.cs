namespace TaSked.Api.ApiClient;

public class HomeworkApiOptions
{
	public string BaseUrl { get; set; } = "localhost";
	public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);
	public bool UseNotifications = true;
}
