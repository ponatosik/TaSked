using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.SubjectTests;

[Collection("Database tests")]
public class ChangeSubjectNameCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly ChangeSubjectNameCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId;

    public ChangeSubjectNameCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new ChangeSubjectNameCommandHandler(_context);

        var user = User.Create(UserHelper.GenerateUniqueUserName());
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
        var newSubjectName = "Updated subject name";
        var command = new ChangeSubjectNameCommand(_userId, _subjectId, newSubjectName);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(newSubjectName,
            _context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Name);
    }
}