using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.MemberTests;

[Collection("Persistance tests")]
public class GetGroupMembersHandlerTest
{
    private readonly IApplicationDbContext _context;
    private readonly GetGroupMembersHandler _handler;

    private readonly Guid _userId, _groupId;
    private readonly List<User> _users = new List<User>();

    public GetGroupMembersHandlerTest(PersistanceFixture persistanceFixture)
    {
        _context = persistanceFixture.GetDbContext();
        _handler = new GetGroupMembersHandler(_context);

        User user = User.Create("Test user");
        Group group = Group.Create("Test group", user);

        _userId = user.Id;
        _groupId = group.Id;

        _users.Add(user);
        _users.Add(User.Create("Test user 2"));
        _users.Add(User.Create("Test user 3"));

        _users[1].JoinGroup(group);
        _users[2].JoinGroup(group);

        _context.Users.Add(user);
        _context.Groups.Add(group);
        _context.SaveChangesAsync(new CancellationToken()).Wait();
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnGroupMembers()
    {
        var request = new GetGroupMembersQuery(_userId, _groupId);

        var result = await _handler.Handle(request, new CancellationToken());

        Assert.Equal(_users.Count, result.Count);
        Assert.Contains(result, member => member.Id == _users[0].Id);
        Assert.Contains(result, member => member.Id == _users[1].Id);
        Assert.Contains(result, member => member.Id == _users[2].Id);
    }
}