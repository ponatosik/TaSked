using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.SubjectTests;

[Collection("Database tests")]
public class GetAllSubjectsCommandHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly GetAllSubjectsHandler _handler;

    private readonly Guid _userId;
    private readonly List<Subject> _subjects = new List<Subject>();

    public GetAllSubjectsCommandHandlerTest(DbTestFixture dbTestFixture)
    {
        _context = dbTestFixture.GetDbContext();
        _handler = new GetAllSubjectsHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);

        _userId = user.Id;

        _subjects.Add(group.CreateSubject("test subject 1"));
        _subjects.Add(group.CreateSubject("test subject 2"));
        _subjects.Add(group.CreateSubject("test subject 3"));

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(CancellationToken.None).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSubjects()
    {
        var request = new GetAllSubjectsQuery(_userId);

        var result = await _handler.Handle(request, CancellationToken.None);

        Assert.Equal(_subjects.Count, result.Count);
        Assert.Collection(result.OrderBy(r => r.Name),
                subject => Assert.Equal(_subjects[0].Name, subject.Name),
                subject => Assert.Equal(_subjects[1].Name, subject.Name),
                subject => Assert.Equal(_subjects[2].Name, subject.Name)
            );
    }
}