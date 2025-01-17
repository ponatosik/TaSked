using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.InvitationTests;

[Collection("Persistance tests")]
public class ActivateInvitationCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ActivateInvitationCommandHandler _handler;

    private Guid _userId, _groupId, _inviationId;

    public ActivateInvitationCommandHandlerTest(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new ActivateInvitationCommandHandler(_context);

        var groupAdmin = User.Create("Test admin");
        var group = Group.Create("Test group", groupAdmin);
        var invitation = group.CreateInvintation("Test invitation");

        var user = User.Create("Test user");

        _userId = user.Id;
        _groupId = group.Id;
        _inviationId = invitation.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(new CancellationToken()).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var command = new ActivateInvitationCommand(_userId, _inviationId, _groupId);

        await _handler.Handle(command, new CancellationToken());

        Assert.Contains(_context.Groups.First(g => g.Id == _groupId).Members, user => user.Id == _userId);
    }
}