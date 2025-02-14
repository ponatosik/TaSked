using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.GroupTests;

[Collection("Database tests")]
public class DeleteGroupCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly DeleteGroupCommandHandler _handler;

    private readonly Guid _userId, _groupId;

    public DeleteGroupCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new DeleteGroupCommandHandler(_context);

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
        var command = new DeleteGroupCommand(_userId);

        await _handler.Handle(command, CancellationToken.None);

        Assert.DoesNotContain(_context.Groups, group => group.Id == _groupId);
    }
}