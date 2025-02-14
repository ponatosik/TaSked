using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;

namespace Application.UsersTest;

[Collection("Database tests")]
public class CreateUserCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new CreateUserCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
	    var userNickname = UserHelper.GenerateUniqueUserName();
        var command = new CreateUserCommand(userNickname);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Contains(_context.Users, user => user.Nickname == userNickname);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnUser()
    {
	    var userNickname = UserHelper.GenerateUniqueUserName();
        var command = new CreateUserCommand(userNickname);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result is not null);
        Assert.Equal(userNickname, result.Nickname);
    }
}