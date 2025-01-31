using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.GroupTests;

[Collection("Database tests")]
public class CreateGroupCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly CreateGroupCommandHandler _handler;

    private readonly Guid _userId;

    public CreateGroupCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new CreateGroupCommandHandler(_context);

        User user = User.Create("Test user");
        _userId = user.Id;

        _context.Users.Add(user);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var groupName = "Test group";
        var command = new CreateGroupCommand(_userId, groupName);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Contains(_context.Groups, group => group.Name == groupName);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnGroup()
    {
        var groupName = "Test group";
        var command = new CreateGroupCommand(_userId, groupName);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result is not null);
        Assert.Equal(groupName, result.Name);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPromoteCreatorToAdministrator()
    {
        var command = new CreateGroupCommand(_userId, "Test group");

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(_context.Users.First(u => u.Id == _userId).Role, GroupRole.Admin);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldAddCreatorToGroup()
    {
        var command = new CreateGroupCommand(_userId, "Test group");

        var group = await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(_context.Users.First(u => u.Id == _userId).GroupId, group.Id);
    }
}