using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.LessonTests;

[Collection("Persistance tests")]
public class DeleteLessonCommandHandlerTets
{
    private readonly IApplicationDbContext _context;
    private readonly DeleteLessonCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId, _lessonId;

    public DeleteLessonCommandHandlerTets(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new DeleteLessonCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);
        Subject subject = group.CreateSubject("Test subject");
        Lesson lesson = subject.CreateLesson(DateTime.Now);

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
        var command = new DeleteLessonCommand(_userId, _subjectId, _lessonId);

        await _handler.Handle(command, new CancellationToken());

        Assert.DoesNotContain(_context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Lessons
            , lesson => lesson.Id == _lessonId);
    }
}