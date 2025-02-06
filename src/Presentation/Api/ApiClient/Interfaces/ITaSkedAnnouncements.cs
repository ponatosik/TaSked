using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedAnnouncements
{
	[Post("/Announcements")]
	public Task<Announcement> CreateAnnouncement([Body] CreateAnnouncementRequest request);

	[Get("/Announcements")]
	public Task<List<Announcement>> GetAllAnnouncements();
}
