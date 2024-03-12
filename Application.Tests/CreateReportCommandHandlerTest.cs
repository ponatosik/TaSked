using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class CreateReportCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly CreateReportCommandHandler _handler;

	private readonly Guid _userId, _groupId;	

	public CreateReportCommandHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new CreateReportCommandHandler(_context);

		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);
		_userId = user.Id;
		_groupId = group.Id;	
		
		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		var reportTitle = "Test title";
		var reportMessage = "Test message";
		var command = new CreateReportCommand(_userId, reportTitle, reportMessage);
		
		await _handler.Handle(command, new CancellationToken());

		Assert.Contains(_context.Groups.First(g => g.Id == _groupId).Reports
			, report => report.Title == reportTitle && report.Message == reportMessage);
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnSubject() 
	{
		var reportTitle = "Test title";
		var reportMessage = "Test message";
		var command = new CreateReportCommand(_userId, reportTitle, reportMessage);
		
		var result = await _handler.Handle(command, new CancellationToken());

		Assert.True(result is not null);
		Assert.Equal(reportTitle, result.Title);
		Assert.Equal(reportMessage, result.Message);
	}
}