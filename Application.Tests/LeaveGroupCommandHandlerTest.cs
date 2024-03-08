using Application.Tests;
using TaSked.Domain;
using TaSked.Application.Data;
using NuGet.Frameworks;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class LeaveGroupCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly LeaveGroupCommandHandler _handler;

	private Guid _userId, _groupId;

	public LeaveGroupCommandHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
        _handler = new LeaveGroupCommandHandler(_context);

		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);

		_userId = user.Id;
		_groupId = group.Id;

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		var command = new LeaveGroupCommand(_userId);
		
		await _handler.Handle(command, new CancellationToken());

		Assert.DoesNotContain(_context.Users, user => user.Id == _userId);
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldUpdateUser() 
	{
		var command = new LeaveGroupCommand(_userId);
		
		await _handler.Handle(command, new CancellationToken());

		var user = _context.Users.First(user => user.Id == _userId);
		
		Assert.Equal(GroupRole.NoGroup, _context.Users.First(user => user.Id == _userId).Role);
		Assert.Null(_context.Users.First(user => user.Id == _userId).GroupId);
	}
}