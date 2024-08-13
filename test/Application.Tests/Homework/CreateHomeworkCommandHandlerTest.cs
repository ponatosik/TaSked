using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Persistance tests")]
public class CreateHomeworkCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly CreateHomeworkCommandHandler _handler;

    private readonly Guid _userId, _groupId, _subjectId;

    public CreateHomeworkCommandHandlerTest(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new CreateHomeworkCommandHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);
        Subject subject = group.CreateSubject("Test subject");

        _userId = user.Id;
        _groupId = group.Id;
        _subjectId = subject.Id;

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(new CancellationToken()).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldPersistChanges()
    {
        var homeworkTitle = "Test homerork";
        var homeworkDescription = "Test description";
        var command = new CreateHomeworkCommand(_userId, _subjectId, homeworkTitle, homeworkDescription);

        await _handler.Handle(command, new CancellationToken());

        Assert.Contains(_context
            .Groups.First(group => group.Id == _groupId)
            .Subjects.First(subject => subject.Id == _subjectId)
            .Homeworks
            , homework => homework.Title == homeworkTitle && homework.Description == homeworkDescription);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnHomework()
    {
        var homeworkTitle = "Test homerork";
        var homeworkDescription = "Test description";
        var command = new CreateHomeworkCommand(_userId, _subjectId, homeworkTitle, homeworkDescription);

        var result = await _handler.Handle(command, new CancellationToken());

        Assert.True(result is not null);
        Assert.Equal(homeworkTitle, result.Title);
        Assert.Equal(homeworkDescription, result.Description);
    }
}