using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Database tests")]
public class DeleteHomeworkCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly DeleteHomeworkCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId, _homeworkId;

    public DeleteHomeworkCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new DeleteHomeworkCommandHandler(_context);

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
        var command = new DeleteHomeworkCommand(_userId, _subjectId, _homeworkId);

        await _handler.Handle(command, CancellationToken.None);

        Assert.DoesNotContain(_context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Homeworks
            , homework => homework.Id == _homeworkId);
    }
}