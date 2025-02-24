using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Database tests")]
public class ChangeHomeworkDeadlineCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeHomeworkDeadlineCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId, _homeworkId;

    public ChangeHomeworkDeadlineCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new ChangeHomeworkDeadlineCommandHandler(_context);

        var user = User.Create(UserHelper.GenerateUniqueUserName());
        Group group = Group.Create("Test group", user);
        Subject subject = group.CreateSubject("Test subject");
        Homework homework = subject.CreateHomework("Test homework", "Test description");

        _userId = user.Id;
        _groupId = group.Id;
        _subjectId = subject.Id;
        _homeworkId = homework.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var newDeadline = DateTime.Parse("2011-03-21 13:26Z").ToUniversalTime();
        var command = new ChangeHomeworkDeadlineCommand(_userId, _subjectId, _homeworkId, newDeadline);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(newDeadline,
            _context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Homeworks.First(homework => homework.Id == _homeworkId)
            .Deadline);
    }
    
    [Fact]
    public async Task Handle_ValidCommandAndLocalTimezone_ShouldPersistChangesInUtc()
    {
        var newDeadline = DateTime.Parse("2011-03-21 13:26+02:00");
        var command = new ChangeHomeworkDeadlineCommand(_userId, _subjectId, _homeworkId, newDeadline);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(newDeadline.ToUniversalTime(),
            _context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Homeworks.First(homework => homework.Id == _homeworkId)
            .Deadline);
    }
}