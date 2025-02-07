using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.AnnouncementTests;

[Collection("Database tests")]
public class GetAllAnnouncementsQueryHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly GetAllAnnouncementHandler _handler;

    private readonly Guid _userId;
    private readonly List<Announcement> _announcements = new();

    public GetAllAnnouncementsQueryHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new GetAllAnnouncementHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);

        _userId = user.Id;

        _announcements.Add(group.CreateAnnouncement("test announcement 1", "test message 1"));
        _announcements.Add(group.CreateAnnouncement("test announcement 2", "test message 2"));
        _announcements.Add(group.CreateAnnouncement("test announcement 3", "test message 3"));

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSubjects()
    {
	    var request = new GetAllAnnouncementsQuery(_userId);

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(_announcements.Count, result.Count);
        Assert.Collection(result.OrderBy(r => r.Title),
	        report => Assert.Equal(_announcements[0].Title, report.Title),
	        report => Assert.Equal(_announcements[1].Title, report.Title),
	        report => Assert.Equal(_announcements[2].Title, report.Title)
            );
    }
}