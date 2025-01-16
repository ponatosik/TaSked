using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.InvitationTests;

[Collection("Persistance tests")]
public class ExpireInvitationCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ExpireInvitationCommandHandler _handler;

    private Guid _groupId, _inviationId, _userId;

    public ExpireInvitationCommandHandlerTest(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new ExpireInvitationCommandHandler(_context);

        var groupAdmin = User.Create("Test admin");
        var group = Group.Create("Test group", groupAdmin);
        var invitation = group.CreateInvitation("Test invitation");

        _groupId = group.Id;
        _inviationId = invitation.Id;
        _userId = groupAdmin.Id;

        _context.Groups.Add(group);
        _context.Users.Add(groupAdmin);
        _context.SaveChangesAsync(new CancellationToken()).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var command = new ExpireInvitationCommand(_userId, _inviationId);

        await _handler.Handle(command, new CancellationToken());

        Assert.True(_context.Groups
            .First(g => g.Id == _groupId).Invitations
            .First(inv => inv.Id == _inviationId)
            .IsExpired);
    }
}