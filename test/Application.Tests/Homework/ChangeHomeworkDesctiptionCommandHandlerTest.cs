using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Database tests")]
public class ChangeHomeworkDesctiptionCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeHomeworkDescriptionCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId, _homeworkId;

    public ChangeHomeworkDesctiptionCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new ChangeHomeworkDescriptionCommandHandler(_context);

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
        var newDescription = "new description";
        var command = new ChangeHomeworkDescriptionCommand(_userId, _subjectId, _homeworkId, newDescription);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(newDescription,
            _context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Homeworks.First(homework => homework.Id == _homeworkId)
            .Description);
    }
}