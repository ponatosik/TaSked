using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.SubjectTests;

[Collection("Database tests")]
public class DeleteSubjectCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly DeleteSubjectCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId;

    public DeleteSubjectCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new DeleteSubjectCommandHandler(_context);

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
        var command = new DeleteSubjectCommand(_userId, _subjectId);

        await _handler.Handle(command, CancellationToken.None);

        Assert.DoesNotContain(_context
            .Groups.First(group => group.Id == _groupId)
            .Subjects,
            subject => subject.Id == _subjectId);
    }
}