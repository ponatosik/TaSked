using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.SubjectTests;

[Collection("Database tests")]
public class CreateSubjectCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly CreateSubjectCommandHandler _handler;

    private readonly Guid _userId, _groupId;

    public CreateSubjectCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new CreateSubjectCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);
        _userId = user.Id;
        _groupId = group.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var subjectName = "Test subject";
        var command = new CreateSubjectCommand(_userId, subjectName);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Contains(_context.Groups.First(g => g.Id == _groupId).Subjects, subj => subj.Name == subjectName);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSubject()
    {
        var subjectName = "Test subject";
        var command = new CreateSubjectCommand(_userId, subjectName);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result is not null);
        Assert.Equal(subjectName, result.Name);
    }
}