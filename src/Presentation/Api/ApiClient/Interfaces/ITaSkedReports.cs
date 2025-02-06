using Refit;
using TaSked.Api.Requests;
using TaSked.Domain;

namespace TaSked.Api.ApiClient;

public interface ITaSkedReports
{
	[Post("/Reports")]
	public Task<Report> CreateReport([Body] CreateReportRequest request);

	[Get("/Reports")]
	public Task<List<Report>> GetAllReports();
}
