using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class GetAllHomeworkQueryHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly GetAllHomeworkHandler _handler;

	private readonly Guid _userId;
	private readonly List<Homework> _homeworks = new List<Homework>();

	public GetAllHomeworkQueryHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new GetAllHomeworkHandler(_context);

		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);
		Subject subject1 = group.CreateSubject("test subject 1");
		Subject subject2 = group.CreateSubject("test subject 2");

		_userId = user.Id;

		_homeworks.Add(subject1.CreateHomework("homework 1", "for subject 1"));
		_homeworks.Add(subject1.CreateHomework("homework 2", "for subject 1"));
		_homeworks.Add(subject2.CreateHomework("homework 1", "for subject 2"));

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnSubjects()
	{
		var request = new GetAllHomeworkQuery(_userId);

		var result = await _handler.Handle(request, new CancellationToken());

		Assert.Equal(_homeworks.Count, result.Count);
		Assert.Contains(result, homework => homework.Id == _homeworks[0].Id);
		Assert.Contains(result, homework => homework.Id == _homeworks[1].Id);
		Assert.Contains(result, homework => homework.Id == _homeworks[2].Id);
	}
}