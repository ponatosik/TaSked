using Application.Tests;
using Microsoft.EntityFrameworkCore;
using TaSked.Application.Data;
using TaSked.Domain;

namespace TaSked.Application.Tests;

[Collection("Persistance tests")]
public class GetAllLessonsBySubjectQueryHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly GetAllLessonsBySubjectHandler _handler;

	private readonly Guid _userId;
	private readonly Guid _subjectId;
	private readonly List<Lesson> _lessons = new List<Lesson>();

	public GetAllLessonsBySubjectQueryHandlerTest(PersistanceFixture persistanceFixture)
	{
		_context = persistanceFixture.GetDbContext();
		_handler = new GetAllLessonsBySubjectHandler(_context);

		User user = User.Create("Test user");
		Group group = Group.Create("Test group", user);
		Subject subject1 = group.CreateSubject("test subject 1");
		Subject subject2 = group.CreateSubject("test subject 2");

		_userId = user.Id;
		_subjectId = subject1.Id;	

		_lessons.Add(subject1.CreateLesson(DateTime.Parse("2011-03-21 13:20")));
		_lessons.Add(subject1.CreateLesson(DateTime.Parse("2011-03-21 14:40")));
		subject2.CreateLesson(DateTime.Parse("2011-03-21 15:50"));

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(new CancellationToken()).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldReturnLessonOfSubject()
	{
		var request = new GetAllLessonsBySubjectQuery(_userId, _subjectId);

		var result = await _handler.Handle(request, new CancellationToken());

		Assert.Equal(_lessons.Count, result.Count);
		Assert.Contains(result, lesson => lesson.Id == _lessons[0].Id);
		Assert.Contains(result, lesson => lesson.Id == _lessons[1].Id);
	}
}