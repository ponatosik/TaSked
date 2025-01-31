using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.InvitationTests;

[Collection("Database tests")]
public class GetInvitationInfoQueryHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly GetInvitationInfoHandler _handler;

    private readonly Guid _userId, _groupId;

    public GetInvitationInfoQueryHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new GetInvitationInfoHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);

        _userId = user.Id;
        _groupId = group.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnInvitation()
    {
        var invitationCaption = "Test invitation";
        var id = _context.Groups.First(group => group.Id == _groupId).CreateInvitation(invitationCaption).Id;
        await _context.SaveChangesAsync(CancellationToken.None);
        var query = new GetInvitationInfoQuery(id);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(invitationCaption, result.Caption);
    }
}