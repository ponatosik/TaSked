using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class GetInvitationInfoQueryHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly GetInvitationInfoHandler _handler;

	private readonly Guid _userId, _groupId;

	public GetInvitationInfoQueryHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new GetInvitationInfoHandler(_context);

		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);

		_userId = user.Id;
		_groupId = group.Id;

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnInvitation()
	{
		var invitationCaption = "Test invitation";
		var id  = _context.Groups.First(group => group.Id == _groupId).CreateInvintation(invitationCaption).Id;
		await _context.SaveChangesAsync(new CancellationToken());
		var query = new GetInvitationInfoQuery(id);

		var result = await _handler.Handle(query, new CancellationToken());
		
		Assert.Equal(invitationCaption, result.Caption);
	}
}