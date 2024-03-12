using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class GetAllLessonsInDateRangeQueryHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly GetAllLessonsInDateRangeHandler _handler;

	private readonly Guid _userId;
	private readonly Guid _subjectId;
	private readonly List<Lesson> _lessons = new List<Lesson>();

	public GetAllLessonsInDateRangeQueryHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new GetAllLessonsInDateRangeHandler(_context);

		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);
		Subject subject1 = group.CreateSubject("test subject 1");
		Subject subject2 = group.CreateSubject("test subject 2");

		_userId = user.Id;
		_subjectId = subject1.Id;

		subject1.CreateLesson(DateTime.Parse("2011-03-21 13:20"));
		_lessons.Add(subject1.CreateLesson(DateTime.Parse("2011-03-21 14:40")));
		_lessons.Add(subject2.CreateLesson(DateTime.Parse("2011-03-21 15:50")));

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnLessonInDateRange()
	{
		var from = DateTime.Parse("2011-03-21 14:40");
		var to = DateTime.Parse("2011-03-21 15:50");
		var request = new GetAllLessonsInDateRangeQuery(_userId, from, to);

		var result = await _handler.Handle(request, new CancellationToken());

		Assert.Equal(_lessons.Count, result.Count);
		Assert.Contains(result, lesson => lesson.Id == _lessons[0].Id);
		Assert.Contains(result, lesson => lesson.Id == _lessons[1].Id);
	}
}