using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class CreateGroupCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly CreateGroupCommandHandler _handler;

	private readonly Guid _userId;

	public CreateGroupCommandHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new CreateGroupCommandHandler(_context);

		User user = User.Create("Test user");
		_userId = user.Id;
		
		_context.Users.Add(user);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		var groupName = "Test group";
		var command = new CreateGroupCommand(_userId, groupName);
		
		await _handler.Handle(command, new CancellationToken());

		Assert.Contains(_context.Groups, group => group.Name == groupName);
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnGroup() 
	{
		var groupName = "Test group";
		var command = new CreateGroupCommand(_userId, groupName);
		
		var result = await _handler.Handle(command, new CancellationToken());

		Assert.True(result is not null);
		Assert.Equal(groupName, result.Name);
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPromoteCreatorToAdministrator()
	{
		var command = new CreateGroupCommand(_userId, "Test group");

		await _handler.Handle(command, new CancellationToken());

		Assert.Equal(_context.Users.First(u => u.Id == _userId).Role, GroupRole.Admin);
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldAddCreatorToGroup()
	{
		var command = new CreateGroupCommand(_userId, "Test group");

		var group = await _handler.Handle(command, new CancellationToken());

		Assert.Equal(_context.Users.First(u => u.Id == _userId).GroupId, group.Id);
	}
}