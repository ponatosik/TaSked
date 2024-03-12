using Application.Tests;
using TaSked.Application.Data;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class CreateUserCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly CreateUserCommandHandler _handler;

	public CreateUserCommandHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new CreateUserCommandHandler(_context);
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		var userNickname = "Test user";
		var command = new CreateUserCommand(userNickname);
		
		await _handler.Handle(command, new CancellationToken());

		Assert.Contains(_context.Users, user => user.Nickname == userNickname);
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnUser() 
	{
		var userNickname = "Test user";
		var command = new CreateUserCommand(userNickname);
		
		var result = await _handler.Handle(command, new CancellationToken());

		Assert.True(result is not null);
		Assert.Equal(userNickname, result.Nickname);
	}
}