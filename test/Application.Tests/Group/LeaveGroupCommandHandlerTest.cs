using Application.Tests;
using TaSked.Domain;
using TaSked.Application.Data;
using NuGet.Frameworks;
using TaSked.Application;

namespace Application.GroupTests;

[Collection("Database tests")]
public class LeaveGroupCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly LeaveGroupCommandHandler _handler;

    private Guid _userId, _groupId;

    public LeaveGroupCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new LeaveGroupCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);

        _userId = user.Id;
        _groupId = group.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var command = new LeaveGroupCommand(_userId);

        await _handler.Handle(command, CancellationToken.None);

        Assert.DoesNotContain(_context.Groups.First(group => group.Id == _groupId).Members, user => user.Id == _userId);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldUpdateUser()
    {
        var command = new LeaveGroupCommand(_userId);

        await _handler.Handle(command, CancellationToken.None);

        var user = _context.Users.First(user => user.Id == _userId);

        Assert.Equal(GroupRole.NoGroup, _context.Users.First(user => user.Id == _userId).Role);
        Assert.Null(_context.Users.First(user => user.Id == _userId).GroupId);
    }
}