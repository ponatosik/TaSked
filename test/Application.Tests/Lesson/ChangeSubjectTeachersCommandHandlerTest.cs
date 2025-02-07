using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.LessonTests;

[Collection("Database tests")]
public class ChangeSubjectTeachersCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeSubjectTeacherCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId;

    public ChangeSubjectTeachersCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new ChangeSubjectTeacherCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);
        var lessonUrl = RelatedLink.Create(new Uri("https://meeting/join"));
        var teacher = Teacher.Create("Test", "Test", "Test", "Test", lessonUrl);
        var subject = group.CreateSubject("Test subject", [teacher]);

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
	    List<Teacher> newTeachers =
	    [
		    Teacher.Create("updated", "updated", "updated", "updated", newLessonUrl),
		    Teacher.Create("new", "new", "new", "new", newLessonUrl)
	    ];
	    var command =
		    new ChangeSubjectTeachersCommand(_userId, _subjectId, newTeachers.Select(UpdateTeacherDTO.From).ToList());

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equivalent(newTeachers.Select(t => new { t.FullName, t.Description, t.Email, t.PhoneNumber }),
            _context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Teachers);
    }
}