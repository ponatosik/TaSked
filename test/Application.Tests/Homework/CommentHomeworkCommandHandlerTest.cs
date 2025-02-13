using Application.Tests;
using TaSked.Application;
using TaSked.Application.Data;
using TaSked.Domain;

namespace Application.HomeworkTests;

[Collection("Database tests")]
public class CommentHomeworkCommandHandlerTest
{
	private readonly IApplicationDbContext _context;
	private readonly CommentHomeworkCommandHandler _handler;

	private readonly Guid _userId, _groupId, _subjectId, _homeworkId;

	public CommentHomeworkCommandHandlerTest(DbTestFixture dbTestFixture)
	{
		_context = dbTestFixture.GetDbContext();
		_handler = new CommentHomeworkCommandHandler(_context);

		var user = User.Create(UserHelper.GenerateUniqueUserName());
		var group = Group.Create("Test group", user);
		var subject = group.CreateSubject("Test subject");
		var homework = subject.CreateHomework("Test homework", "Test description");

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
		var commentContent = "Test comment";
		var command = new CommentHomeworkCommand(_userId, _subjectId, _homeworkId, commentContent);

		var comment = await _handler.Handle(command, CancellationToken.None);

		Assert.Equal(commentContent, comment.Content);
		Assert.Contains(_context
			.Groups.First(group => group.Id == _groupId)
			.Subjects.First(subject => subject.Id == _subjectId)
			.Homeworks.First(homework => homework.Id == _homeworkId)
			.Comments, c => c.Id == comment.Id && c.Content == commentContent);
	}
}