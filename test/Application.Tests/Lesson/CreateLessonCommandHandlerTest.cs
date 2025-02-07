using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.LessonTests;

[Collection("Database tests")]
public class CreateLessonCommandHandlerTets
{
    private readonly IApplicationDbContext _context;
    private readonly CreateLessonCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId;

    public CreateLessonCommandHandlerTets(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new CreateLessonCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);
        Subject subject = group.CreateSubject("Test subject");

        _userId = user.Id;
        _groupId = group.Id;
        _subjectId = subject.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var lessonTime = DateTime.Parse("2011-03-21 13:26");
        var command = new CreateLessonCommand(_userId, _subjectId, lessonTime);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Contains(_context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Lessons
            , lesson => lesson.Time == lessonTime);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnLesson()
    {
        var lessonTime = DateTime.Parse("2011-03-21 13:26");
        var command = new CreateLessonCommand(_userId, _subjectId, lessonTime);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result is not null);
        Assert.Equal(lessonTime, result.Time);
    }
}