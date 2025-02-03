using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.LessonTests;

[Collection("Database tests")]
public class ChangeLessonTeacherCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeSubjectTeacherCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId;

    public ChangeLessonTeacherCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new ChangeSubjectTeacherCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);
        var lessonUrl = RelatedLink.Create(new Uri("https://meeting/join"));
        var teacher = Teacher.Create("Test", "Test", "Test", "Test", lessonUrl);
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
	    var newLessonUrl = RelatedLink.Create(new Uri("https://meeting-updated/join"));
	    var newTeacher = Teacher.Create("updated", "updated", "updated", "updated", newLessonUrl);
        var command = new ChangeSubjectTeacherCommand(_userId, _subjectId, newTeacher);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(newTeacher,
            _context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Teacher);
    }
}