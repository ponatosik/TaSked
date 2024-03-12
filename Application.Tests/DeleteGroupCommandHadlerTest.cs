using Application.Tests;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class DeleteGroupCommandHadlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly DeleteGroupCommandHandler _handler;

	private readonly Guid _userId, _groupId;

	public DeleteGroupCommandHadlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new DeleteGroupCommandHandler(_context);
	
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
		var command = new DeleteGroupCommand(_userId);
		
		await _handler.Handle(command, new CancellationToken());
		
		Assert.DoesNotContain(_context.Groups, (group => group.Id == _groupId));
	}
}