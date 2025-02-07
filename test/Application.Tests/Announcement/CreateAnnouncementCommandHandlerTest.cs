using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.AnnouncementTests;

[Collection("Database tests")]
public class CreateAnnouncementCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly CreateAnnouncementCommandHandler _handler;

    private readonly Guid _userId, _groupId;

    public CreateAnnouncementCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new CreateAnnouncementCommandHandler(_context);

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
	    var announcementTitle = "Test title";
	    var announcementMessage = "Test message";
	    var command = new CreateAnnouncementCommand(_userId, announcementTitle, announcementMessage);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Contains(_context.Groups.First(g => g.Id == _groupId).Announcements
	        , announcement => announcement.Title == announcementTitle && announcement.Message == announcementMessage);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSubject()
    {
	    var announcementTitle = "Test title";
	    var announcementMessage = "Test message";
	    var command = new CreateAnnouncementCommand(_userId, announcementTitle, announcementMessage);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result is not null);
        Assert.Equal(announcementTitle, result.Title);
        Assert.Equal(announcementMessage, result.Message);
    }
}