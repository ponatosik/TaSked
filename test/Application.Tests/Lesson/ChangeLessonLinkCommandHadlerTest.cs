using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.LessonTests;

[Collection("Persistance tests")]
public class ChangeLessonRelatedLinksCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeLessonLinkCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId, _lessonId;

    public ChangeLessonRelatedLinksCommandHandlerTest(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new ChangeLessonLinkCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);
        Subject subject = group.CreateSubject("Test subject");
        Lesson lesson = subject.CreateLesson(DateTime.Parse("2011-03-21 13:26"));

        _userId = user.Id;
        _groupId = group.Id;
        _subjectId = subject.Id;
        _lessonId = lesson.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(new CancellationToken()).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
	    var newLink = RelatedLink.Create(new Uri("http://zoom.com/test"), "Zoom lecture");
	    var command = new ChangeLessonLinkCommand(_userId, _subjectId, _lessonId, newLink);

        await _handler.Handle(command, new CancellationToken());

        Assert.Equal(newLink,
            _context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Lessons.First(lesson => lesson.Id == _lessonId)
            .OnlineLessonUrl);
    }
}