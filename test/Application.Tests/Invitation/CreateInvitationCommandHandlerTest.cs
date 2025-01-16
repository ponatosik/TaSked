using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.InvitationTests;

[Collection("Persistance tests")]
public class CreateInvitationCommandHandlerTets
{
    private readonly IApplicationDbContext _context;
    private readonly CreateInvitationCommandHandler _handler;

    private readonly Guid _userId, _groupId;

    public CreateInvitationCommandHandlerTets(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new CreateInvitationCommandHandler(_context);

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
        var invitationCaption = "Test invitation";
        var command = new CreateInvitationCommand(_userId, invitationCaption);

        await _handler.Handle(command, new CancellationToken());

        Assert.Contains(_context
            .Groups.First(group => group.Id == _groupId)
            .Invitations
            , invitation => invitation.Caption!.Equals(invitationCaption));
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnInvitation()
    {
        var invitationCaption = "Test invitation";
        var command = new CreateInvitationCommand(_userId, invitationCaption);

        var result = await _handler.Handle(command, new CancellationToken());

        Assert.True(result is not null);
        Assert.Equal(invitationCaption, result.Caption);
    }
}