using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Database tests")]
public class ChangeHomeworkBriefSummaryCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly ChangeHomeworkBriefSummaryCommandHandler _handler;

	private readonly Guid _userId, _groupId, _subjectId, _homeworkId;

	public ChangeHomeworkBriefSummaryCommandHandlerTest(DbTestFixture dbTestFixture)
	{
		_context = dbTestFixture.GetDbContext();
		_handler = new ChangeHomeworkBriefSummaryCommandHandler(_context);

		var user = User.Create("Test user");
		var group = Group.Create("Test group", user);
		var subject = group.CreateSubject("Test subject");
		var homework = subject.CreateHomework("Test homework", "Test description", briefSummary: "summary");

		_userId = user.Id;
		_groupId = group.Id;
		_subjectId = subject.Id;
		_homeworkId = homework.Id;

		_context.Users.Add(user);
		_context.Groups.Add(group);
		_context.SaveChangesAsync(CancellationToken.None).Wait();
	}

	[Fact]
	public async Task Handle_ValidCommand_ShouldPersistChanges()
	{
		var newBriefSummary = "new brief summary";
		var command = new ChangeHomeworkBriefSummaryCommand(_userId, _subjectId, _homeworkId, newBriefSummary);

		await _handler.Handle(command, CancellationToken.None);

		Assert.Equal(newBriefSummary,
			_context
				.Groups.First(group => group.Id == _groupId)
				.Subjects.First(subject => subject.Id == _subjectId)
				.Homeworks.First(homework => homework.Id == _homeworkId)
				.BriefSummary);
	}
}