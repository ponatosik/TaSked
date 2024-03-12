using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class GetAllReportsQuetyHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly GetAllReportHandler _handler;

	private readonly Guid _userId;
	private readonly List<Report> _reports = new List<Report>();

	public GetAllReportsQuetyHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new GetAllReportHandler(_context);

		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);

		_userId = user.Id;

		_reports.Add(group.CreateReport("test report 1", "test message 1"));
		_reports.Add(group.CreateReport("test report 2", "test message 2"));
		_reports.Add(group.CreateReport("test report 3", "test message 3"));
		
		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnSubjects()
	{
		var request = new GetAllReportQuery(_userId);

		var result = await _handler.Handle(request, new CancellationToken());

		Assert.Equal(_reports.Count, result.Count);
		Assert.Collection(result,
			report => Assert.Equal(_reports[0].Id, report.Id),
			report => Assert.Equal(_reports[1].Id, report.Id),
			report => Assert.Equal(_reports[2].Id, report.Id));
	}
}