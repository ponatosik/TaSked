using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.GroupTests;

[Collection("Persistance tests")]
public class ChangeGroupNameCommandHadlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeGroupNameCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId;

    public ChangeGroupNameCommandHadlerTest(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new ChangeGroupNameCommandHandler(_context);

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
        var newName = "updated";
        var command = new ChangeGroupNameCommand(_userId, newName);

        await _handler.Handle(command, new CancellationToken());

        Assert.Equal(newName,
            _context
            .Groups.First(group => group.Id == _groupId)
            .Name);
    }
}