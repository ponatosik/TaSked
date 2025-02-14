using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.GroupTests;

[Collection("Database tests")]
public class ChangeGroupNameCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeGroupNameCommandHandler _handler;

    private readonly Guid _userId, _groupId;

    public ChangeGroupNameCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new ChangeGroupNameCommandHandler(_context);

        var user = User.Create(UserHelper.GenerateUniqueUserName());
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
        var newName = "updated";
        var command = new ChangeGroupNameCommand(_userId, newName);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(newName,
            _context
            .Groups.First(group => group.Id == _groupId)
            .Name);
    }
}